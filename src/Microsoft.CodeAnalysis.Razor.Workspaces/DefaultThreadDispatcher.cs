// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Microsoft.CodeAnalysis.Razor.Workspaces
{
    internal class DefaultThreadDispatcher : ThreadDispatcher
    {
        private readonly Thread _thread;

        public DefaultThreadDispatcher()
        {
            _thread = Thread.CurrentThread;
        }

        protected override bool IsForegroundThread()
        {
            return Thread.CurrentThread == _thread;
        }
    }
}
