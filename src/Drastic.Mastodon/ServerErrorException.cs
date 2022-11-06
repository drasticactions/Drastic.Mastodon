// <copyright file="ServerErrorException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Drastic.Mastodon.Entities;

namespace Drastic.Mastodon
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException(Error error)
            : base(error.Description)
        {
        }
    }
}
