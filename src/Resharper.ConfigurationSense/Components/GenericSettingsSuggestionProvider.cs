﻿using System.Collections.Generic;

using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

using Resharper.ConfigurationSense.Extensions;
using Resharper.ConfigurationSense.Models;

namespace Resharper.ConfigurationSense.Components
{
    [ShellComponent]
    public class GenericSettingsSuggestionProvider : IGenericSettingsProvider
    {
        private readonly ProjectModelElementPresentationService _presentationService;

        public GenericSettingsSuggestionProvider(ProjectModelElementPresentationService presentationService)
        {
            _presentationService = presentationService;
        }

        public LinkedList<KeyValueSettingLookupItem> GetJsonSettingsLookupItems(
            CSharpCodeCompletionContext context,
            string fileName,
            string jsonPath = null)
        {
            var lookupItems = new LinkedList<KeyValueSettingLookupItem>();

            var project = context.BasicContext.File.GetProject();
            if (project == null)
            {
                return lookupItems;
            }

            var rangeMarker = CreateRangeMarker(context);
            var settings = project.GetJsonProjectSettings(jsonPath);

            return CreateLookupItems(context, settings, project, rangeMarker, lookupItems);
        }

        public LinkedList<KeyValueSettingLookupItem> GetXmlSettingsLookupItems(
            CSharpCodeCompletionContext context,
            string settingsTagName,
            string settingsKeyAttribute,
            string settingsValueAttributes)
        {
            var lookupItems = new LinkedList<KeyValueSettingLookupItem>();

            var project = context.BasicContext.File.GetProject();
            if (project == null)
            {
                return lookupItems;
            }

            var rangeMarker = CreateRangeMarker(context);
            var settings = project.GetXmlProjectSettings(settingsTagName, settingsKeyAttribute, settingsValueAttributes);

            return CreateLookupItems(context, settings, project, rangeMarker, lookupItems);
        }

        private LinkedList<KeyValueSettingLookupItem> CreateLookupItems(
            CSharpCodeCompletionContext context,
            IEnumerable<KeyValueSetting> settings,
            IProject project,
            IRangeMarker rangeMarker,
            LinkedList<KeyValueSettingLookupItem> lookupItems)
        {
            foreach (var setting in settings)
            {
                var iconId = _presentationService.GetIcon(project);
                var lookupItem = new KeyValueSettingLookupItem(setting, iconId, rangeMarker);
                lookupItem.InitializeRanges(context.EvaluateRanges(), context.BasicContext);

                lookupItems.AddLast(lookupItem);
            }

            return lookupItems;
        }

        private IRangeMarker CreateRangeMarker(CSharpCodeCompletionContext context)
        {
            var rangeMarker =
                new TextRange(context.BasicContext.CaretDocumentRange.TextRange.StartOffset).CreateRangeMarker(
                    context.BasicContext.Document);
            return rangeMarker;
        }
    }
}
