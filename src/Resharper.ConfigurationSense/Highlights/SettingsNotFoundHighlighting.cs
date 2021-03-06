﻿using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

using Resharper.ConfigurationSense.Highlights;

[assembly:
    RegisterConfigurableSeverity(SettingsNotFoundHighlighting.SeverityId, null, HighlightingGroupIds.CompilerWarnings,
        "The setting wasn't found in configuration files", "The setting wasn't found in configuration files",
        Severity.WARNING)]

namespace Resharper.ConfigurationSense.Highlights
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
    public class SettingsNotFoundHighlighting : IHighlighting
    {
        public const string SeverityId = "SettingNotFoundInConfiguration";

        private readonly IArgumentList _argumentList;

        private readonly ICSharpArgumentsOwner _argumentsOwner;

        public SettingsNotFoundHighlighting(
            ICSharpArgumentsOwner argumentsOwner,
            IArgumentList argumentList,
            string key,
            string type)
        {
            _argumentsOwner = argumentsOwner;
            _argumentList = argumentList;
            ToolTip = $"{type} {key} wasn't found in cofiguration files";
        }

        public string ErrorStripeToolTip => ToolTip;

        public string ToolTip { get; }

        public DocumentRange CalculateRange()
        {
            return _argumentList.GetHighlightingRange();
        }

        public bool IsValid()
        {
            if (_argumentsOwner != null)
            {
                return _argumentsOwner.IsValid();
            }

            return true;
        }
    }
}
