using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WitchPantry.EditorTools
{
    public static class SceneIntrospectionExporter
    {
        private const string DefaultExportDirectory = "Exports/SceneIntrospection";

        [MenuItem("Tools/Witch Pantry/Export/Active Scene Introspection")]
        public static void ExportActiveSceneFromMenu()
        {
            var scene = SceneManager.GetActiveScene();
            if (!scene.IsValid() || string.IsNullOrWhiteSpace(scene.path))
            {
                Debug.LogError("Cannot export the active scene because it has not been saved yet.");
                return;
            }

            var export = CaptureScene(scene);
            var outputPath = BuildSceneOutputPath(DefaultExportDirectory, scene.path);
            WriteExport(outputPath, export);
            Debug.Log($"Scene introspection exported: {outputPath}");
        }

        [MenuItem("Tools/Witch Pantry/Export/All Project Scenes Introspection")]
        public static void ExportAllScenesFromMenu()
        {
            ExportScenes(FindAllScenePaths(), DefaultExportDirectory);
        }

        // Entry point for Unity batchmode:
        // -executeMethod WitchPantry.EditorTools.SceneIntrospectionExporter.ExportFromCommandLine --all-scenes
        // -executeMethod WitchPantry.EditorTools.SceneIntrospectionExporter.ExportFromCommandLine --scene=Assets/Scenes/Foo.unity
        // Optional:
        // --output=Exports/SceneIntrospection
        public static void ExportFromCommandLine()
        {
            var arguments = Environment.GetCommandLineArgs();
            var outputDirectory = GetArgumentValue(arguments, "--output") ?? DefaultExportDirectory;
            var requestedScene = GetArgumentValue(arguments, "--scene");
            var exportAllScenes = arguments.Any(arg => string.Equals(arg, "--all-scenes", StringComparison.OrdinalIgnoreCase));

            IReadOnlyList<string> scenePaths;
            if (!string.IsNullOrWhiteSpace(requestedScene))
            {
                scenePaths = new[] { requestedScene };
            }
            else if (exportAllScenes)
            {
                scenePaths = FindAllScenePaths();
            }
            else
            {
                scenePaths = new[] { SceneManager.GetActiveScene().path }.Where(path => !string.IsNullOrWhiteSpace(path)).ToArray();
            }

            ExportScenes(scenePaths, outputDirectory);
        }

        private static void ExportScenes(IReadOnlyList<string> scenePaths, string outputDirectory)
        {
            if (scenePaths == null || scenePaths.Count == 0)
            {
                Debug.LogWarning("No scenes were found to export.");
                return;
            }

            EnsureDirectoryExists(outputDirectory);

            foreach (var scenePath in scenePaths.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                var export = CaptureScene(scene);
                var outputPath = BuildSceneOutputPath(outputDirectory, scenePath);
                WriteExport(outputPath, export);
                Debug.Log($"Scene introspection exported: {outputPath}");
            }
        }

        private static SceneExport CaptureScene(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            return new SceneExport
            {
                sceneName = scene.name,
                scenePath = scene.path,
                exportedAtUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
                rootGameObjects = roots.Select(CaptureGameObject).ToList()
            };
        }

        private static GameObjectExport CaptureGameObject(GameObject gameObject)
        {
            var transform = gameObject.transform;
            var export = new GameObjectExport
            {
                name = gameObject.name,
                hierarchyPath = BuildHierarchyPath(transform),
                tag = gameObject.tag,
                layer = gameObject.layer,
                isActiveSelf = gameObject.activeSelf,
                isActiveInHierarchy = gameObject.activeInHierarchy,
                isStatic = gameObject.isStatic,
                scenePath = gameObject.scene.path,
                transform = CaptureTransform(transform),
                components = gameObject.GetComponents<Component>().Select(CaptureComponent).ToList(),
                children = new List<GameObjectExport>()
            };

            for (var i = 0; i < transform.childCount; i++)
            {
                export.children.Add(CaptureGameObject(transform.GetChild(i).gameObject));
            }

            return export;
        }

        private static TransformExport CaptureTransform(Transform transform)
        {
            return new TransformExport
            {
                localPosition = CaptureVector3(transform.localPosition),
                localRotation = CaptureVector3(transform.localEulerAngles),
                localScale = CaptureVector3(transform.localScale),
                worldPosition = CaptureVector3(transform.position),
                worldRotation = CaptureVector3(transform.eulerAngles),
                siblingIndex = transform.GetSiblingIndex()
            };
        }

        private static ComponentExport CaptureComponent(Component component)
        {
            if (component == null)
            {
                return new ComponentExport
                {
                    typeName = "MissingComponent",
                    serializedProperties = new List<SerializedPropertyExport>()
                };
            }

            var export = new ComponentExport
            {
                typeName = component.GetType().FullName,
                serializedProperties = CaptureSerializedProperties(component)
            };

            if (component is MonoBehaviour monoBehaviour)
            {
                var script = MonoScript.FromMonoBehaviour(monoBehaviour);
                if (script != null)
                {
                    export.scriptAssetPath = AssetDatabase.GetAssetPath(script);
                    export.scriptGuid = AssetDatabase.AssetPathToGUID(export.scriptAssetPath);
                }
            }

            return export;
        }

        private static List<SerializedPropertyExport> CaptureSerializedProperties(Component component)
        {
            var properties = new List<SerializedPropertyExport>();
            var serializedObject = new SerializedObject(component);
            var iterator = serializedObject.GetIterator();
            var enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                properties.Add(new SerializedPropertyExport
                {
                    name = iterator.name,
                    path = iterator.propertyPath,
                    type = iterator.propertyType.ToString(),
                    depth = iterator.depth,
                    value = ReadPropertyValue(iterator)
                });

                enterChildren = false;
            }

            return properties;
        }

        private static string ReadPropertyValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.longValue.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return property.doubleValue.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Color:
                    return ColorUtility.ToHtmlStringRGBA(property.colorValue);
                case SerializedPropertyType.ObjectReference:
                    return FormatObjectReference(property.objectReferenceValue);
                case SerializedPropertyType.LayerMask:
                    return property.intValue.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.Enum:
                    return property.enumDisplayNames != null &&
                           property.enumValueIndex >= 0 &&
                           property.enumValueIndex < property.enumDisplayNames.Length
                        ? property.enumDisplayNames[property.enumValueIndex]
                        : property.enumValueIndex.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.Vector2:
                    return FormatVector2(property.vector2Value);
                case SerializedPropertyType.Vector3:
                    return FormatVector3(property.vector3Value);
                case SerializedPropertyType.Vector4:
                    return FormatVector4(property.vector4Value);
                case SerializedPropertyType.Rect:
                    return property.rectValue.ToString();
                case SerializedPropertyType.ArraySize:
                    return property.intValue.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.Character:
                    return ((char)property.intValue).ToString();
                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue != null ? property.animationCurveValue.ToString() : string.Empty;
                case SerializedPropertyType.Bounds:
                    return property.boundsValue.ToString();
                case SerializedPropertyType.Gradient:
                    return property.gradientValue != null ? property.gradientValue.ToString() : string.Empty;
                case SerializedPropertyType.Quaternion:
                    return FormatQuaternion(property.quaternionValue);
                case SerializedPropertyType.ExposedReference:
                    return FormatObjectReference(property.exposedReferenceValue);
                case SerializedPropertyType.FixedBufferSize:
                    return property.fixedBufferSize.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.Vector2Int:
                    return property.vector2IntValue.ToString();
                case SerializedPropertyType.Vector3Int:
                    return property.vector3IntValue.ToString();
                case SerializedPropertyType.RectInt:
                    return property.rectIntValue.ToString();
                case SerializedPropertyType.BoundsInt:
                    return property.boundsIntValue.ToString();
                case SerializedPropertyType.ManagedReference:
                    return string.IsNullOrWhiteSpace(property.managedReferenceFullTypename)
                        ? "null"
                        : property.managedReferenceFullTypename;
                case SerializedPropertyType.Hash128:
                    return property.hash128Value.ToString();
                default:
                    return string.Empty;
            }
        }

        private static string FormatObjectReference(UnityEngine.Object reference)
        {
            if (reference == null)
            {
                return "null";
            }

            var assetPath = AssetDatabase.GetAssetPath(reference);
            if (!string.IsNullOrWhiteSpace(assetPath))
            {
                return $"{reference.name} ({reference.GetType().Name}) @ {assetPath}";
            }

            if (reference is Component component)
            {
                return $"{BuildHierarchyPath(component.transform)} ({component.GetType().Name})";
            }

            if (reference is GameObject gameObject)
            {
                return $"{BuildHierarchyPath(gameObject.transform)} (GameObject)";
            }

            return $"{reference.name} ({reference.GetType().Name})";
        }

        private static string BuildHierarchyPath(Transform transform)
        {
            var segments = new Stack<string>();
            var current = transform;

            while (current != null)
            {
                segments.Push(current.name);
                current = current.parent;
            }

            return string.Join("/", segments);
        }

        private static string BuildSceneOutputPath(string outputDirectory, string scenePath)
        {
            var sceneName = Path.GetFileNameWithoutExtension(scenePath);
            return Path.Combine(outputDirectory, $"{SanitizeFileName(sceneName)}.scene.json");
        }

        private static void WriteExport(string outputPath, SceneExport export)
        {
            EnsureDirectoryExists(Path.GetDirectoryName(outputPath));
            var json = JsonUtility.ToJson(export, true);
            File.WriteAllText(outputPath, json);
            AssetDatabase.Refresh();
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        private static IReadOnlyList<string> FindAllScenePaths()
        {
            return AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static string GetArgumentValue(IEnumerable<string> arguments, string key)
        {
            foreach (var argument in arguments)
            {
                if (argument.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
                {
                    return argument.Substring(key.Length + 1).Trim('"');
                }
            }

            return null;
        }

        private static Vector3Export CaptureVector3(Vector3 value)
        {
            return new Vector3Export
            {
                x = value.x,
                y = value.y,
                z = value.z
            };
        }

        private static string FormatVector2(Vector2 value) => $"({value.x}, {value.y})";
        private static string FormatVector3(Vector3 value) => $"({value.x}, {value.y}, {value.z})";
        private static string FormatVector4(Vector4 value) => $"({value.x}, {value.y}, {value.z}, {value.w})";
        private static string FormatQuaternion(Quaternion value) => $"({value.x}, {value.y}, {value.z}, {value.w})";

        private static string SanitizeFileName(string fileName)
        {
            foreach (var invalidCharacter in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }

            return fileName;
        }

        [Serializable]
        private sealed class SceneExport
        {
            public string sceneName;
            public string scenePath;
            public string exportedAtUtc;
            public List<GameObjectExport> rootGameObjects;
        }

        [Serializable]
        private sealed class GameObjectExport
        {
            public string name;
            public string hierarchyPath;
            public string tag;
            public int layer;
            public bool isActiveSelf;
            public bool isActiveInHierarchy;
            public bool isStatic;
            public string scenePath;
            public TransformExport transform;
            public List<ComponentExport> components;
            public List<GameObjectExport> children;
        }

        [Serializable]
        private sealed class TransformExport
        {
            public Vector3Export localPosition;
            public Vector3Export localRotation;
            public Vector3Export localScale;
            public Vector3Export worldPosition;
            public Vector3Export worldRotation;
            public int siblingIndex;
        }

        [Serializable]
        private sealed class Vector3Export
        {
            public float x;
            public float y;
            public float z;
        }

        [Serializable]
        private sealed class ComponentExport
        {
            public string typeName;
            public string scriptAssetPath;
            public string scriptGuid;
            public List<SerializedPropertyExport> serializedProperties;
        }

        [Serializable]
        private sealed class SerializedPropertyExport
        {
            public string name;
            public string path;
            public string type;
            public int depth;
            public string value;
        }
    }
}
