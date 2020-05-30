using System;
using Microsoft.Azure.WebJobs.Description;

namespace AuthExample.AuthTokenBinding
{
    /// <summary>
    /// A custom attribute that lets you pass an <see cref="AccessTokenResult"/> into an function definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public sealed class AccessTokenAttribute : Attribute
    {
    }
}
