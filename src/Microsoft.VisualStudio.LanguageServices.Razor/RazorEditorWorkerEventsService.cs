// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor
{
    public class RazorEditorWorkerEventsService
    {
        public abstract void GetWorker(ITextBuffer razorBuffer);

        public abstract void AdviseWorkerEvents(IRazorEditorWorkerEvents events);
    }
}
