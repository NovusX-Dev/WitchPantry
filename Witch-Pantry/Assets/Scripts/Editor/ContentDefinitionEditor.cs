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
        private const string RegenerateIdButtonLabel = "Regenerate ID";
        private const string ValidateAllButtonLabel = "Validate All";
        private static readonly string[] InspectorPropertiesToExclude =
        {
            ContentDefinitionEditorUtility.ScriptPropertyName,
            ContentDefinitionEditorUtility.UnlockSourceBackingFieldName,
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, InspectorPropertiesToExclude);
            DrawUnlockSourceInspector();
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
                if (GUILayout.Button(RegenerateIdButtonLabel))
                {
                    ContentDefinitionEditorUtility.RegenerateId(definition);
                }

                if (GUILayout.Button(ValidateAllButtonLabel))
                {
                    ContentDefinitionEditorUtility.ValidateAllContentDefinitions(logResults: true);
                }
            }
        }

        private void DrawUnlockSourceInspector()
        {
            var unlockSourceProperty = ContentDefinitionEditorUtility.GetUnlockSourceProperty(serializedObject);
            if (unlockSourceProperty == null)
            {
                return;
            }

            var unlockTypeProperty = unlockSourceProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockSourceTypeFieldName);
            var sourceIdProperty = unlockSourceProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockSourceIdFieldName);
            if (unlockTypeProperty == null || sourceIdProperty == null)
            {
                EditorGUILayout.PropertyField(unlockSourceProperty, includeChildren: true);
                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(unlockTypeProperty);
            if (EditorGUI.EndChangeCheck())
            {
                ContentDefinitionEditorUtility.ApplyUnlockSourceTypeSelection(unlockTypeProperty, sourceIdProperty);
            }

            var selectedUnlockType = (GlobalConstants.UnlockSourceType)unlockTypeProperty.enumValueIndex;
            var sourceIdShouldBeDisabled = selectedUnlockType == GlobalConstants.UnlockSourceType.StartingContent;
            if (sourceIdShouldBeDisabled && !string.IsNullOrWhiteSpace(sourceIdProperty.stringValue))
            {
                sourceIdProperty.stringValue = string.Empty;
            }

            using (new EditorGUI.DisabledScope(sourceIdShouldBeDisabled))
            {
                EditorGUILayout.PropertyField(sourceIdProperty);
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
        private const string ToolsMenuRoot = "Tools/Witch Pantry/Validation/";
        private const string FixMenuItemPath = ToolsMenuRoot + "Fix Content Definitions";
        private const string ValidateMenuItemPath = ToolsMenuRoot + "Validate Content Definitions";
        private const string ContentDefinitionAssetFilter = "t:" + nameof(ContentDefinition);
        private const string DefaultIdPrefix = "content.";
        private const string DefaultSlug = "unnamed";
        private const string IdSeparator = ".";
        public const string ScriptPropertyName = "m_Script";
        public static readonly string UnlockSourceBackingFieldName = GetAutoPropertyBackingFieldName(nameof(IngredientDefinition.UnlockSource));
        public static readonly string UnlockSourceTypeFieldName = nameof(UnlockSource.Type);
        public static readonly string UnlockSourceIdFieldName = nameof(UnlockSource.SourceId);
        public static readonly string RecipeOutputsBackingFieldName = GetAutoPropertyBackingFieldName(nameof(RecipeDefinition.Outputs));
        public static readonly string OutputAmountDefinitionFieldName = nameof(OutputAmount.output);
        public static readonly string OutputAmountValueFieldName = nameof(OutputAmount.amount);

        private static readonly string IdBackingField = GetAutoPropertyBackingFieldName(nameof(ContentDefinition.Id));
        private static readonly string ContentTypeBackingField = GetAutoPropertyBackingFieldName(nameof(ContentDefinition.ContentType));
        private static readonly string DisplayNameBackingField = GetAutoPropertyBackingFieldName(nameof(ContentDefinition.DisplayName));
        private static readonly IReadOnlyDictionary<GlobalConstants.UnlockSourceType, string> UnlockSourcePrefixes =
            new Dictionary<GlobalConstants.UnlockSourceType, string>
            {
                { GlobalConstants.UnlockSourceType.Biome, BuildIdPrefix("biome") },
                { GlobalConstants.UnlockSourceType.Machine, BuildIdPrefix(nameof(GlobalConstants.ContentType.Machine)) },
                { GlobalConstants.UnlockSourceType.Recipe, BuildIdPrefix(nameof(GlobalConstants.ContentType.Recipe)) },
                { GlobalConstants.UnlockSourceType.ContractReward, BuildIdPrefix("contract") },
                { GlobalConstants.UnlockSourceType.Research, BuildIdPrefix("research") },
                { GlobalConstants.UnlockSourceType.Prestige, BuildIdPrefix("prestige") },
                { GlobalConstants.UnlockSourceType.EventReward, BuildIdPrefix("event") },
            };

        [MenuItem(FixMenuItemPath)]
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

        [MenuItem(ValidateMenuItemPath)]
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

            changed |= InitializeDisplayName(serializedObject, definition.name);
            changed |= NormalizeUnlockSource(serializedObject);
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
            return AssetDatabase.FindAssets(ContentDefinitionAssetFilter)
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

        private static bool InitializeDisplayName(SerializedObject serializedObject, string generatedName)
        {
            var property = serializedObject.FindProperty(DisplayNameBackingField);
            if (property == null || !string.IsNullOrWhiteSpace(property.stringValue))
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

        public static SerializedProperty GetUnlockSourceProperty(SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(UnlockSourceBackingFieldName);
        }

        public static void ApplyUnlockSourceTypeSelection(
            SerializedProperty unlockTypeProperty,
            SerializedProperty sourceIdProperty)
        {
            if (unlockTypeProperty == null || sourceIdProperty == null)
            {
                return;
            }

            var selectedUnlockType = (GlobalConstants.UnlockSourceType)unlockTypeProperty.enumValueIndex;
            if (selectedUnlockType == GlobalConstants.UnlockSourceType.StartingContent)
            {
                sourceIdProperty.stringValue = string.Empty;
                return;
            }

            var expectedPrefix = GetUnlockSourcePrefix(selectedUnlockType);
            if (string.IsNullOrWhiteSpace(expectedPrefix))
            {
                return;
            }

            var currentSourceId = sourceIdProperty.stringValue ?? string.Empty;
            if (string.IsNullOrWhiteSpace(currentSourceId))
            {
                sourceIdProperty.stringValue = expectedPrefix;
                return;
            }

            var suffix = TrimKnownUnlockSourcePrefix(currentSourceId);
            sourceIdProperty.stringValue = expectedPrefix + suffix;
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
                issues.Add("DisplayName is required.");
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

            issues.AddRange(ValidateUnlockSource(definition));
            issues.AddRange(ValidateRecipeOutputs(definition));

            return issues;
        }

        private static IEnumerable<string> ValidateUnlockSource(ContentDefinition definition)
        {
            if (!TryGetUnlockSource(definition, out var unlockSource))
            {
                yield break;
            }

            if (unlockSource.Type == GlobalConstants.UnlockSourceType.StartingContent)
            {
                if (!string.IsNullOrWhiteSpace(unlockSource.SourceId))
                {
                    yield return "UnlockSource.SourceId must be empty when UnlockSource.Type is StartingContent.";
                }

                yield break;
            }

            if (string.IsNullOrWhiteSpace(unlockSource.SourceId))
            {
                yield return $"UnlockSource.SourceId is required when UnlockSource.Type is {unlockSource.Type}.";
                yield break;
            }

            var expectedPrefix = GetUnlockSourcePrefix(unlockSource.Type);
            if (!string.IsNullOrWhiteSpace(expectedPrefix) &&
                !unlockSource.SourceId.StartsWith(expectedPrefix, StringComparison.Ordinal))
            {
                yield return $"UnlockSource.SourceId should start with `{expectedPrefix}` when UnlockSource.Type is {unlockSource.Type}.";
            }
        }

        private static bool TryGetUnlockSource(ContentDefinition definition, out UnlockSource unlockSource)
        {
            switch (definition)
            {
                case IngredientDefinition ingredientDefinition:
                    unlockSource = ingredientDefinition.UnlockSource;
                    return true;
                case PotionDefinition potionDefinition:
                    unlockSource = potionDefinition.UnlockSource;
                    return true;
                default:
                    unlockSource = default;
                    return false;
            }
        }

        private static string GetUnlockSourcePrefix(GlobalConstants.UnlockSourceType unlockSourceType)
        {
            return UnlockSourcePrefixes.TryGetValue(unlockSourceType, out var prefix)
                ? prefix
                : string.Empty;
        }

        private static IEnumerable<string> ValidateRecipeOutputs(ContentDefinition definition)
        {
            if (!(definition is RecipeDefinition recipeDefinition))
            {
                yield break;
            }

            var outputs = recipeDefinition.Outputs;
            if (outputs == null || outputs.Length == 0)
            {
                yield return "RecipeDefinition must define at least one output.";
                yield break;
            }

            for (var index = 0; index < outputs.Length; index++)
            {
                var outputAmount = outputs[index];
                if (outputAmount.output == null)
                {
                    yield return $"RecipeDefinition.Outputs[{index}] is missing an output reference.";
                    continue;
                }

                if (!IsAllowedRecipeOutputType(outputAmount.output))
                {
                    yield return $"RecipeDefinition.Outputs[{index}] uses invalid output type `{outputAmount.output.GetType().Name}`. Recipes may only output IngredientDefinition or PotionDefinition assets.";
                }

                if (outputAmount.amount <= 0)
                {
                    yield return $"RecipeDefinition.Outputs[{index}] must have an amount greater than 0.";
                }
            }
        }

        private static bool IsAllowedRecipeOutputType(ContentDefinition outputDefinition)
        {
            return outputDefinition is IngredientDefinition || outputDefinition is PotionDefinition;
        }

        private static bool NormalizeUnlockSource(SerializedObject serializedObject)
        {
            var unlockSourceProperty = GetUnlockSourceProperty(serializedObject);
            if (unlockSourceProperty == null)
            {
                return false;
            }

            var unlockTypeProperty = unlockSourceProperty.FindPropertyRelative(UnlockSourceTypeFieldName);
            var sourceIdProperty = unlockSourceProperty.FindPropertyRelative(UnlockSourceIdFieldName);
            if (unlockTypeProperty == null || sourceIdProperty == null)
            {
                return false;
            }

            var selectedUnlockType = (GlobalConstants.UnlockSourceType)unlockTypeProperty.enumValueIndex;
            if (selectedUnlockType != GlobalConstants.UnlockSourceType.StartingContent)
            {
                var expectedPrefix = GetUnlockSourcePrefix(selectedUnlockType);
                if (string.IsNullOrWhiteSpace(expectedPrefix))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(sourceIdProperty.stringValue))
                {
                    sourceIdProperty.stringValue = expectedPrefix;
                    return true;
                }

                return false;
            }

            if (string.IsNullOrWhiteSpace(sourceIdProperty.stringValue))
            {
                return false;
            }

            sourceIdProperty.stringValue = string.Empty;
            return true;
        }

        private static string TrimKnownUnlockSourcePrefix(string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId))
            {
                return string.Empty;
            }

            foreach (var prefix in UnlockSourcePrefixes.Values)
            {
                if (sourceId.StartsWith(prefix, StringComparison.Ordinal))
                {
                    return sourceId.Substring(prefix.Length);
                }
            }

            return sourceId;
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
                ? BuildIdPrefix(expectedType.Value.ToString())
                : DefaultIdPrefix;
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
                return DefaultSlug;
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
            return string.IsNullOrWhiteSpace(slug) ? DefaultSlug : slug;
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

        private static string BuildIdPrefix(string prefixRoot)
        {
            return prefixRoot.ToLowerInvariant() + IdSeparator;
        }

        private static string GetAutoPropertyBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}
