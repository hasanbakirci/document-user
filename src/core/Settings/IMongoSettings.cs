﻿namespace core.Settings;

public interface IMongoSettings
{
    string Server { get; set; }
    string Database { get; set; }
    string Collection { get; set; }

}