// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language
{
    public sealed class SourceChange
    {
        public SourceChange(int absoluteIndex, int length, string newText)
        {
            if (newText == null)
            {
                throw new ArgumentNullException(nameof(newText));
            }

            Span = new SourceSpan(absoluteIndex, length);
            NewText = NewText;
        }

        public SourceChange(SourceSpan span, string newText)
        {
            if (newText == null)
            {
                throw new ArgumentNullException(nameof(newText));
            }

            Span = span;
            NewText = newText;
        }

        public bool IsInsert => Span.Length == 0 && NewText.Length > 0;

        public bool IsDelete => Span.Length > 0 && NewText.Length == 0;

        public bool IsReplace => Span.Length > 0 && NewText.Length > 0;

        public SourceSpan Span { get; }

        public string NewText { get; }

        internal string GetEditedContent(Span span)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            var offset = GetOffset(span);
            return GetEditedContent(span, offset);
        }

        internal string GetEditedContent(Span span, int offset)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }
            
            return span.Content.Remove(offset, Span.Length).Insert(offset, NewText);
        }

        internal string GetEditedContent(string text, int offset)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.Remove(offset, Span.Length).Insert(offset, NewText);
        }

        internal int GetOffset(Span span)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            var start = Span.AbsoluteIndex;
            var end = Span.AbsoluteIndex + Span.Length;

            if (start < span.Start.AbsoluteIndex ||
                start > span.Start.AbsoluteIndex + span.Length ||
                end < span.Start.AbsoluteIndex || 
                end > span.Start.AbsoluteIndex + span.Length)
            {
                throw new InvalidOperationException(Resources.FormatInvalidOperation_SpanIsNotChangeOwner(span, this));
            }

            return start - span.Start.AbsoluteIndex;
        }

        internal string GetOriginalText(Span span)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            if (span.Length == 0)
            {
                return string.Empty;
            }

            var offset = GetOffset(span);
            if (offset == span.Content.Length)
            {
                return string.Empty;
            }

            return span.Content.Substring(offset, Span.Length);
        }
    }
}
