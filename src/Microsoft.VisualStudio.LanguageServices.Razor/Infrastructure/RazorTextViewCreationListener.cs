// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Export(typeof(IVsTextViewCreationListener))]
    internal class RazorTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        public IVsEditorAdaptersFactoryService AdapterFactory { get; set; }

        [Import]
        public ITextBufferInitializationService Initializer { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var textView = AdapterFactory.GetWpfTextView(textViewAdapter);
            if (textView == null)
            {
                return;
            }

            var razorBuffer = textView.BufferGraph.GetTextBuffers(b => b.ContentType.TypeName == RazorLanguage.ContentType).FirstOrDefault();
            if (razorBuffer != null)
            {
                var listener = new TextBufferStateListener(Initializer, textView.BufferGraph, razorBuffer);
                razorBuffer.Properties.AddProperty(typeof(TextBufferStateListener), listener);
            }
        }
    }
}
