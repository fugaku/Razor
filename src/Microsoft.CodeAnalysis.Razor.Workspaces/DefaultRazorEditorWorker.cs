// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces
{
    internal class DefaultRazorEditorWorker : RazorEditorWorker
    {
        private readonly ThreadDispatcher _dispatcher;
        private readonly Workspace _workspace;
        private readonly DocumentId _documentId;
        private readonly SourceTextContainer _container;

        private State _current;

        public DefaultRazorEditorWorker(ThreadDispatcher dispatcher, Workspace workspace, DocumentId documentId, SourceTextContainer container)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            _dispatcher = dispatcher;
            _workspace = workspace;
            _documentId = documentId;
            _container = container;

            _current = new PrivateState(null, null, null);

            _container.TextChanged += Container_TextChanged;
        }

        private void Container_TextChanged(object sender, TextChangeEventArgs e)
        {
            _dispatcher.AssertForegroundThread();
        }

        public override State Current => _current;
        
        public override Task ParseAsync(TextChange change)
        {
            _dispatcher.AssertForegroundThread();
            return Task.CompletedTask;
        }

        private class PrivateState : State
        {
            private readonly SourceText _text;
            private readonly RazorSyntaxTree _syntaxTree;
            private readonly RazorCSharpDocument _cSharpDocument;

            public PrivateState(SourceText text, RazorSyntaxTree syntaxTree, RazorCSharpDocument cSharpDocument)
            {
                _text = text;
                _syntaxTree = syntaxTree;
                _cSharpDocument = cSharpDocument;
            }

            public override SourceText Text => _text;

            public override RazorSyntaxTree SyntaxTree => _syntaxTree;

            public override RazorCSharpDocument CSharpDocument => _cSharpDocument;
        }
    }
}
