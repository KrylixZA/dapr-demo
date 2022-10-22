using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Attributes;

/// <summary>
/// A validation attribute to ensure a property is not default.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MustNotBeDefaultAttribute : ValidationAttribute
{
  /// <summary>
  /// Instantiates a new instance of the MustNotBeDefaultAttribute class.
  /// </summary>
  public MustNotBeDefaultAttribute()
      : base("The {0} field must not be the default value.")
  {
  }

  /// <inheritdoc />
  public override bool IsValid(object? value)
  {
    if (value is null)
    {
      return false;
    }

    var type = value.GetType();
    return !Equals(value, Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type));
  }
}