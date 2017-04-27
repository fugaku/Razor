// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor
{
    public class CompilationTagHelperFeature : ITagHelperFeature
    {
        private RazorEngine _engine;
        private ITagHelperDescriptorProvider[] _providers;
        private IMetadataReferenceFeature _referenceFeature;
        
        public RazorEngine Engine
        {
            get
            {
                return _engine;
            }
            set
            {
                _engine = value;
                OnInitialized();
            }
        }

        public IReadOnlyList<TagHelperDescriptor> GetDescriptors()
        {
            var results = new List<TagHelperDescriptor>();

            var context = new Context(results);
            var compilation = CSharpCompilation.Create("__TagHelpers", references: _referenceFeature.References);
            context.SetCompilation(compilation);

            for (var i = 0; i < _providers.Length; i++)
            {
                _providers[i].Execute(context);
            }

            return results;
        }

        private void OnInitialized()
        {
            _referenceFeature = Engine.Features.OfType<IMetadataReferenceFeature>().FirstOrDefault();
            _providers = Engine.Features.OfType<ITagHelperDescriptorProvider>().OrderBy(f => f.Order).ToArray();
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
