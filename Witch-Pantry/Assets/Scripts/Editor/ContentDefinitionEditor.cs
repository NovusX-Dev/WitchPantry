using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using WitchPantry.Data;

namespace WitchPantry.Editor
{
    [CustomEditor(typeof(ContentDefinition), true)]
    public sealed class ContentDefinitionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();

            var definition = (ContentDefinition)target;
            ContentDefinitionEditorUtility.SyncDefinition(definition);

            var issues = ContentDefinitionEditorUtility.ValidateDefinition(definition);
            if (issues.Count > 0)
            {
                EditorGUILayout.HelpBox(string.Join(Environment.NewLine, issues), MessageType.Warning);
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Regenerate ID"))
                {
                    ContentDefinitionEditorUtility.RegenerateId(definition);
                }

                if (GUILayout.Button("Validate All"))
                {
                    ContentDefinitionEditorUtility.ValidateAllContentDefinitions(logResults: true);
                }
            }
        }
    }

    public sealed class ContentDefinitionSaveHook : AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                var definition = AssetDatabase.LoadAssetAtPath<ContentDefinition>(path);
                if (definition == null)
                {
                    continue;
                }

                ContentDefinitionEditorUtility.SyncDefinition(definition);
            }

            return paths;
        }
    }

    public static class ContentDefinitionEditorUtility
    {
        private const string IdBackingField = "<Id>k__BackingField";
        private const string ContentTypeBackingField = "<ContentType>k__BackingField";
        private const string ContentNameBackingField = "<ContentName>k__BackingField";

        [MenuItem("Tools/Witch Pantry/Validation/Fix Content Definitions")]
        public static void FixAllContentDefinitions()
        {
            var definitions = LoadAllContentDefinitions();
            var updatedCount = 0;

            foreach (var definition in definitions)
            {
                if (SyncDefinition(definition))
                {
                    updatedCount++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"ContentDefinition fix-up complete. Updated {updatedCount} asset(s).");
        }

        [MenuItem("Tools/Witch Pantry/Validation/Validate Content Definitions")]
        public static void ValidateAllContentDefinitionsMenu()
        {
            ValidateAllContentDefinitions(logResults: true);
        }

        public static bool RegenerateId(ContentDefinition definition)
        {
            return SyncDefinition(definition, forceIdRegeneration: true);
        }

        public static bool SyncDefinition(ContentDefinition definition, bool forceIdRegeneration = false)
        {
            if (definition == null)
            {
                return false;
            }

            var serializedObject = new SerializedObject(definition);
            var changed = false;
            var expectedContentType = GetExpectedContentType(definition.GetType());

            if (expectedContentType.HasValue)
            {
                changed |= SetContentType(serializedObject, expectedContentType.Value);
            }

            changed |= SetContentName(serializedObject, definition.name);
            changed |= SetId(serializedObject, GenerateId(definition), forceIdRegeneration);

            if (!changed)
            {
                return false;
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);
            return true;
        }

        public static IReadOnlyList<string> ValidateDefinition(ContentDefinition definition)
        {
            var duplicateMap = FindDuplicateIds();
            return ValidateDefinitionAgainstMap(definition, duplicateMap);
        }

        public static bool ValidateAllContentDefinitions(bool logResults)
        {
            var definitions = LoadAllContentDefinitions();
            var duplicateMap = FindDuplicateIds(definitions);
            var hasIssues = false;

            foreach (var definition in definitions)
            {
                var issues = ValidateDefinitionAgainstMap(definition, duplicateMap);
                if (issues.Count == 0)
                {
                    continue;
                }

                hasIssues = true;
                if (logResults)
                {
                    var assetPath = AssetDatabase.GetAssetPath(definition);
                    Debug.LogWarning(BuildIssueLog(assetPath, issues), definition);
                }
            }

            if (logResults && !hasIssues)
            {
                Debug.Log($"Validated {definitions.Count} ContentDefinition asset(s). No issues found.");
            }

            return !hasIssues;
        }

        private static IReadOnlyList<ContentDefinition> LoadAllContentDefinitions()
        {
            return AssetDatabase.FindAssets("t:ContentDefinition")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ContentDefinition>)
                .Where(definition => definition != null)
                .OrderBy(definition => definition.name, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static bool SetContentType(SerializedObject serializedObject, GlobalConstants.ContentType contentType)
        {
            var property = serializedObject.FindProperty(ContentTypeBackingField);
            if (property == null || property.enumValueIndex == (int)contentType)
            {
                return false;
            }

            property.enumValueIndex = (int)contentType;
            return true;
        }

        private static bool SetContentName(SerializedObject serializedObject, string generatedName)
        {
            var property = serializedObject.FindProperty(ContentNameBackingField);
            if (property == null || string.Equals(property.stringValue, generatedName, StringComparison.Ordinal))
            {
                return false;
            }

            property.stringValue = generatedName;
            return true;
        }

        private static bool SetId(SerializedObject serializedObject, string generatedId, bool forceIdRegeneration)
        {
            var property = serializedObject.FindProperty(IdBackingField);
            if (property == null)
            {
                return false;
            }

            if (!forceIdRegeneration && !string.IsNullOrWhiteSpace(property.stringValue))
            {
                return false;
            }

            if (string.Equals(property.stringValue, generatedId, StringComparison.Ordinal))
            {
                return false;
            }

            property.stringValue = generatedId;
            return true;
        }

        private static IReadOnlyList<string> ValidateDefinitionAgainstMap(
            ContentDefinition definition,
            IReadOnlyDictionary<string, List<ContentDefinition>> duplicateMap)
        {
            var issues = new List<string>();
            if (definition == null)
            {
                issues.Add("Definition reference is missing.");
                return issues;
            }

            var expectedContentType = GetExpectedContentType(definition.GetType());
            if (expectedContentType.HasValue && definition.ContentType != expectedContentType.Value)
            {
                issues.Add($"ContentType must be {expectedContentType.Value}.");
            }

            if (string.IsNullOrWhiteSpace(definition.DisplayName))
            {
                issues.Add("ContentName is required.");
            }

            var expectedId = GenerateId(definition);
            if (string.IsNullOrWhiteSpace(definition.Id))
            {
                issues.Add("Id is missing.");
            }
            else if (!string.Equals(definition.Id, expectedId, StringComparison.Ordinal))
            {
                issues.Add($"Id should follow the canonical format `{expectedId}`.");
            }

            if (!string.IsNullOrWhiteSpace(definition.Id) &&
                duplicateMap.TryGetValue(definition.Id, out var duplicates) &&
                duplicates.Count > 1)
            {
                issues.Add($"Duplicate Id `{definition.Id}` is used by {duplicates.Count} content assets.");
            }

            return issues;
        }

        private static Dictionary<string, List<ContentDefinition>> FindDuplicateIds()
        {
            return FindDuplicateIds(LoadAllContentDefinitions());
        }

        private static Dictionary<string, List<ContentDefinition>> FindDuplicateIds(IEnumerable<ContentDefinition> definitions)
        {
            return definitions
                .Where(definition => !string.IsNullOrWhiteSpace(definition.Id))
                .GroupBy(definition => definition.Id, StringComparer.Ordinal)
                .ToDictionary(group => group.Key, group => group.ToList(), StringComparer.Ordinal);
        }

        private static string GenerateId(ContentDefinition definition)
        {
            var prefix = GetIdPrefix(definition.GetType());
            var source = !string.IsNullOrWhiteSpace(definition.DisplayName)
                ? definition.DisplayName
                : definition.name;

            return prefix + Slugify(source);
        }

        private static string GetIdPrefix(Type type)
        {
            var expectedType = GetExpectedContentType(type);
            return expectedType.HasValue
                ? expectedType.Value.ToString().ToLowerInvariant() + "."
                : "content.";
        }

        private static GlobalConstants.ContentType? GetExpectedContentType(Type type)
        {
            if (typeof(IngredientDefinition).IsAssignableFrom(type))
            {
                return GlobalConstants.ContentType.Ingredient;
            }

            if (typeof(MachineDefinition).IsAssignableFrom(type))
            {
                return GlobalConstants.ContentType.Machine;
            }

            if (typeof(RecipeDefinition).IsAssignableFrom(type))
            {
                return GlobalConstants.ContentType.Recipe;
            }

            if (typeof(PotionDefinition).IsAssignableFrom(type))
            {
                return GlobalConstants.ContentType.Potion;
            }

            if (typeof(ContractDefinition).IsAssignableFrom(type))
            {
                return GlobalConstants.ContentType.Contract;
            }

            return null;
        }

        private static string Slugify(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "unnamed";
            }

            var builder = new StringBuilder(value.Length);
            var previousWasSeparator = false;

            foreach (var character in value.Trim().ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                    previousWasSeparator = false;
                    continue;
                }

                if (previousWasSeparator)
                {
                    continue;
                }

                builder.Append('_');
                previousWasSeparator = true;
            }

            var slug = builder.ToString().Trim('_');
            return string.IsNullOrWhiteSpace(slug) ? "unnamed" : slug;
        }

        private static string BuildIssueLog(string assetPath, IReadOnlyList<string> issues)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"ContentDefinition issues in {assetPath}:");
            foreach (var issue in issues)
            {
                builder.Append("- ");
                builder.AppendLine(issue);
            }

            return builder.ToString();
        }
    }
}
