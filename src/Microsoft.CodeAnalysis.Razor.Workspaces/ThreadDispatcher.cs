// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.CodeAnalysis.Razor.Workspaces
{
    internal abstract class ThreadDispatcher : IDisposable
    {
        private bool _isDisposed;

        public void AssertForegroundThread([CallerMemberName] string caller = null)
        {
            if (!IsForegroundThread())
            {
throw new InvalidOperationException(Resources.FormatInvalidThreading_ForegroundThreadRequired(caller));
            }
        }

        public void AssertBackgroundThread([CallerMemberName] string caller = null)
        {
            if (IsForegroundThread())
            {
                throw new InvalidOperationException(Resources.FormatInvalidThreading_BackgroundThreadRequired(caller));
            }
        }

        public void AssertAnyThread([CallerMemberName] string caller = null)
        {
            // Do nothing
        }

        protected abstract bool IsForegroundThread();

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                _isDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
