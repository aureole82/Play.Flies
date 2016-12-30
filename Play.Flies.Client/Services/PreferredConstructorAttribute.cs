using System;

namespace Play.Flies.Client.Services
{
    [AttributeUsage(AttributeTargets.Constructor)]
    internal class PreferredConstructorAttribute : Attribute
    {
    }
}