// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor
{
    internal class DefaultTagHelperResolver : TagHelperResolver
    {
        public DefaultTagHelperResolver(bool designTime)
        {
            DesignTime = designTime;
        }

        public bool DesignTime { get; }

        public override TagHelperResolutionResult GetTagHelpers(Compilation compilation, IEnumerable<string> assemblyNameFilters)
        {
            var descriptors = new List<TagHelperDescriptor>();

            var providers = new ITagHelperDescriptorProvider[]
            {
                new DefaultTagHelperDescriptorProvider(),
                new ViewComponentTagHelperDescriptorProvider(),
            };

            var results = new List<TagHelperDescriptor>();
            var context = new Context(results);

            for (var i = 0; i < providers.Length; i++)
            {
                var provider = providers[i];
                provider.Execute(context);
            }
            
            var diagnostics = new List<RazorDiagnostic>();
            var resolutionResult = new TagHelperResolutionResult(results, diagnostics);

            return resolutionResult;
        }

        private class Context : TagHelperDescriptorProviderContext
        {
            private readonly ItemCollection _items;
            private readonly ICollection<TagHelperDescriptor> _results;

            public Context(ICollection<TagHelperDescriptor> results)
            {
                _results = results;

                _items = new DefaultItemCollection();
            }

            public override ItemCollection Items => _items;

            public override ICollection<TagHelperDescriptor> Results => _results;
        }
    }
}