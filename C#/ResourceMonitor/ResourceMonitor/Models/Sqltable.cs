﻿using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models;

public partial class Sqltable
{
    public int Id { get; set; }

    public string TableName { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ServerName { get; set; } = null!;

    public string FriendlyName { get; set; } = null!;

    public string Recipients { get; set; } = null!;

    public string PreviousState { get; set; } = null!;

    public string CurrentState { get; set; } = null!;

    public DateTime LastCheck { get; set; }

    public DateTime LastEmailSent { get; set; }

    public DateTime LastHealthy { get; set; }

    public int EmailFrequency { get; set; }

    public int AcceptableDowntime { get; set; }
}
