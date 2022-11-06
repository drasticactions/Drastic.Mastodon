﻿// <copyright file="MediaDefinition.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Drastic.Mastodon
{
    public class MediaDefinition
    {
        public MediaDefinition(Stream media, string fileName)
        {
            this.Media = media ?? throw new ArgumentException("All the params must be defined", nameof(media));
            this.FileName = fileName ?? throw new ArgumentException("All the params must be defined", nameof(fileName));
        }

        public Stream Media { get; set; }

        public string FileName { get; set; }

        internal string? ParamName { get; set; }
    }
}
