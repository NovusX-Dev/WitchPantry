using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using WitchPantry.Data;
using WitchPantry.Data.ContentDefinition;

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
            ContentDefinitionEditorUtility.UnlockRequirementBackingFieldName,
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, InspectorPropertiesToExclude);
            DrawUnlockRequirementInspector();
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

        private void DrawUnlockRequirementInspector()
        {
            var unlockRequirementProperty = ContentDefinitionEditorUtility.GetUnlockRequirementProperty(serializedObject);
            if (unlockRequirementProperty == null)
            {
                return;
            }

            var unlockTypeProperty =
                unlockRequirementProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockRequirementTypeFieldName);
            var contentDefinitionProperty =
                unlockRequirementProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockRequirementContentDefinitionFieldName);
            var biomeDefinitionProperty =
                unlockRequirementProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockRequirementBiomeDefinitionFieldName);
            var sourceContractProperty =
                unlockRequirementProperty.FindPropertyRelative(ContentDefinitionEditorUtility.UnlockRequirementContractDefinitionFieldName);

            if (unlockTypeProperty == null ||
                contentDefinitionProperty == null ||
                biomeDefinitionProperty == null ||
                sourceContractProperty == null)
            {
                EditorGUILayout.PropertyField(unlockRequirementProperty, includeChildren: true);
                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(unlockTypeProperty);
            if (EditorGUI.EndChangeCheck())
            {
                ContentDefinitionEditorUtility.ApplyUnlockRequirementTypeSelection(
                    unlockTypeProperty,
                    contentDefinitionProperty,
                    biomeDefinitionProperty,
                    sourceContractProperty);
            }

            var selectedUnlockType = (UnlockRequirementType)unlockTypeProperty.enumValueIndex;
            switch (selectedUnlockType)
            {
                case UnlockRequirementType.StartingContent:
                    EditorGUILayout.HelpBox("Starting content requires no additional unlock reference.", MessageType.Info);
                    break;

                case UnlockRequirementType.ContentDefinition:
                    EditorGUILayout.PropertyField(contentDefinitionProperty);
                    break;

                case UnlockRequirementType.Biome:
                    EditorGUILayout.PropertyField(biomeDefinitionProperty);
                    break;

                case UnlockRequirementType.ContractReward:
                    EditorGUILayout.PropertyField(sourceContractProperty);
                    break;

                case UnlockRequirementType.Research:
                case UnlockRequirementType.Prestige:
                case UnlockRequirementType.EventReward:
                    EditorGUILayout.HelpBox(
                        $"{selectedUnlockType} is reserved for later and is not implemented yet. Do not author content with this unlock type yet.",
                        MessageType.Warning);
                    break;

                default:
                    EditorGUILayout.HelpBox($"Unsupported unlock requirement type `{selectedUnlockType}`.", MessageType.Warning);
                    break;
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
        public static readonly string UnlockRequirementBackingFieldName =
            GetAutoPropertyBackingFieldName(nameof(IngredientDefinition.UnlockRequirement));
        public static readonly string UnlockRequirementTypeFieldName = nameof(UnlockRequirement.Type);
        public static readonly string UnlockRequirementContentDefinitionFieldName = nameof(UnlockRequirement.ContentDefinition);
        public static readonly string UnlockRequirementBiomeDefinitionFieldName = nameof(UnlockRequirement.BiomeDefinition);
        public static readonly string UnlockRequirementContractDefinitionFieldName = nameof(UnlockRequirement.SourceContract);
        public static readonly string RecipeOutputsBackingFieldName = GetAutoPropertyBackingFieldName(nameof(RecipeDefinition.Outputs));
        public static readonly string OutputAmountDefinitionFieldName = nameof(OutputAmount.output);
        public static readonly string OutputAmountValueFieldName = nameof(OutputAmount.amount);

        private static readonly string IdBackingField = GetAutoPropertyBackingFieldName(nameof(ContentDefinition.Id));
        private static readonly string ContentTypeBackingField =
            GetAutoPropertyBackingFieldName(nameof(ContentDefinition.ContentType));
        private static readonly string DisplayNameBackingField =
            GetAutoPropertyBackingFieldName(nameof(ContentDefinition.DisplayName));

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
            changed |= NormalizeUnlockRequirement(serializedObject);
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

        public static SerializedProperty GetUnlockRequirementProperty(SerializedObject serializedObject)
        {
            return serializedObject.FindProperty(UnlockRequirementBackingFieldName);
        }

        public static void ApplyUnlockRequirementTypeSelection(
            SerializedProperty unlockTypeProperty,
            SerializedProperty contentDefinitionProperty,
            SerializedProperty biomeDefinitionProperty,
            SerializedProperty sourceContractProperty)
        {
            if (unlockTypeProperty == null ||
                contentDefinitionProperty == null ||
                biomeDefinitionProperty == null ||
                sourceContractProperty == null)
            {
                return;
            }

            var selectedUnlockType = (UnlockRequirementType)unlockTypeProperty.enumValueIndex;
            ClearIrrelevantUnlockRequirementReferences(
                selectedUnlockType,
                contentDefinitionProperty,
                biomeDefinitionProperty,
                sourceContractProperty);
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

            issues.AddRange(ValidateUnlockRequirement(definition));
            issues.AddRange(ValidateRecipeOutputs(definition));

            return issues;
        }

        private static IEnumerable<string> ValidateUnlockRequirement(ContentDefinition definition)
        {
            if (!TryGetUnlockRequirement(definition, out var unlockRequirement))
            {
                yield break;
            }

            switch (unlockRequirement.Type)
            {
                case UnlockRequirementType.StartingContent:
                    foreach (var issue in ValidateUnexpectedUnlockReferences(
                                 unlockRequirement,
                                 expectContentDefinition: false,
                                 expectBiomeDefinition: false,
                                 expectContractDefinition: false))
                    {
                        yield return issue;
                    }
                    yield break;

                case UnlockRequirementType.ContentDefinition:
                    if (unlockRequirement.ContentDefinition == null)
                    {
                        yield return "UnlockRequirement.ContentDefinition is required when UnlockRequirement.Type is ContentDefinition.";
                    }
                    else if (string.IsNullOrWhiteSpace(unlockRequirement.ContentDefinition.Id))
                    {
                        yield return "UnlockRequirement.ContentDefinition must reference a content asset with a valid Id.";
                    }

                    foreach (var issue in ValidateUnexpectedUnlockReferences(
                                 unlockRequirement,
                                 expectContentDefinition: true,
                                 expectBiomeDefinition: false,
                                 expectContractDefinition: false))
                    {
                        yield return issue;
                    }
                    yield break;

                case UnlockRequirementType.Biome:
                    if (unlockRequirement.BiomeDefinition == null)
                    {
                        yield return "UnlockRequirement.BiomeDefinition is required when UnlockRequirement.Type is Biome.";
                    }
                    else if (string.IsNullOrWhiteSpace(unlockRequirement.BiomeDefinition.Id))
                    {
                        yield return "UnlockRequirement.BiomeDefinition must reference a biome asset with a valid Id.";
                    }

                    foreach (var issue in ValidateUnexpectedUnlockReferences(
                                 unlockRequirement,
                                 expectContentDefinition: false,
                                 expectBiomeDefinition: true,
                                 expectContractDefinition: false))
                    {
                        yield return issue;
                    }
                    yield break;

                case UnlockRequirementType.ContractReward:
                    if (unlockRequirement.SourceContract == null)
                    {
                        yield return "UnlockRequirement.SourceContract is required when UnlockRequirement.Type is ContractReward.";
                    }
                    else if (string.IsNullOrWhiteSpace(unlockRequirement.SourceContract.Id))
                    {
                        yield return "UnlockRequirement.SourceContract must reference a contract asset with a valid Id.";
                    }

                    foreach (var issue in ValidateUnexpectedUnlockReferences(
                                 unlockRequirement,
                                 expectContentDefinition: false,
                                 expectBiomeDefinition: false,
                                 expectContractDefinition: true))
                    {
                        yield return issue;
                    }
                    yield break;

                case UnlockRequirementType.Research:
                case UnlockRequirementType.Prestige:
                case UnlockRequirementType.EventReward:
                    yield return $"UnlockRequirement.Type `{unlockRequirement.Type}` is reserved for later and is not implemented yet.";
                    foreach (var issue in ValidateUnexpectedUnlockReferences(
                                 unlockRequirement,
                                 expectContentDefinition: false,
                                 expectBiomeDefinition: false,
                                 expectContractDefinition: false))
                    {
                        yield return issue;
                    }
                    yield break;

                default:
                    yield return $"UnlockRequirement.Type `{unlockRequirement.Type}` is unsupported.";
                    yield break;
            }
        }

        private static IEnumerable<string> ValidateUnexpectedUnlockReferences(
            UnlockRequirement unlockRequirement,
            bool expectContentDefinition,
            bool expectBiomeDefinition,
            bool expectContractDefinition)
        {
            if (!expectContentDefinition && unlockRequirement.ContentDefinition != null)
            {
                yield return "UnlockRequirement.ContentDefinition must be empty for the selected unlock type.";
            }

            if (!expectBiomeDefinition && unlockRequirement.BiomeDefinition != null)
            {
                yield return "UnlockRequirement.BiomeDefinition must be empty for the selected unlock type.";
            }

            if (!expectContractDefinition && unlockRequirement.SourceContract != null)
            {
                yield return "UnlockRequirement.SourceContract must be empty for the selected unlock type.";
            }
        }

        private static bool TryGetUnlockRequirement(ContentDefinition definition, out UnlockRequirement unlockRequirement)
        {
            switch (definition)
            {
                case IngredientDefinition ingredientDefinition:
                    unlockRequirement = ingredientDefinition.UnlockRequirement;
                    return true;
                case PotionDefinition potionDefinition:
                    unlockRequirement = potionDefinition.UnlockRequirement;
                    return true;
                default:
                    unlockRequirement = default;
                    return false;
            }
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

        private static bool NormalizeUnlockRequirement(SerializedObject serializedObject)
        {
            var unlockRequirementProperty = GetUnlockRequirementProperty(serializedObject);
            if (unlockRequirementProperty == null)
            {
                return false;
            }

            var unlockTypeProperty =
                unlockRequirementProperty.FindPropertyRelative(UnlockRequirementTypeFieldName);
            var contentDefinitionProperty =
                unlockRequirementProperty.FindPropertyRelative(UnlockRequirementContentDefinitionFieldName);
            var biomeDefinitionProperty =
                unlockRequirementProperty.FindPropertyRelative(UnlockRequirementBiomeDefinitionFieldName);
            var sourceContractProperty =
                unlockRequirementProperty.FindPropertyRelative(UnlockRequirementContractDefinitionFieldName);

            if (unlockTypeProperty == null ||
                contentDefinitionProperty == null ||
                biomeDefinitionProperty == null ||
                sourceContractProperty == null)
            {
                return false;
            }

            var selectedUnlockType = (UnlockRequirementType)unlockTypeProperty.enumValueIndex;
            return ClearIrrelevantUnlockRequirementReferences(
                selectedUnlockType,
                contentDefinitionProperty,
                biomeDefinitionProperty,
                sourceContractProperty);
        }

        private static bool ClearIrrelevantUnlockRequirementReferences(
            UnlockRequirementType selectedUnlockType,
            SerializedProperty contentDefinitionProperty,
            SerializedProperty biomeDefinitionProperty,
            SerializedProperty sourceContractProperty)
        {
            var changed = false;

            if (selectedUnlockType != UnlockRequirementType.ContentDefinition)
            {
                changed |= ClearObjectReference(contentDefinitionProperty);
            }

            if (selectedUnlockType != UnlockRequirementType.Biome)
            {
                changed |= ClearObjectReference(biomeDefinitionProperty);
            }

            if (selectedUnlockType != UnlockRequirementType.ContractReward)
            {
                changed |= ClearObjectReference(sourceContractProperty);
            }

            return changed;
        }

        private static bool ClearObjectReference(SerializedProperty property)
        {
            if (property == null || property.objectReferenceValue == null)
            {
                return false;
            }

            property.objectReferenceValue = null;
            return true;
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
