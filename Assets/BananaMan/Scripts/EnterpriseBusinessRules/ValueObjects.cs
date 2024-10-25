using UnitGenerator;

namespace BananaMan.EnterpriseBusinessRules;

/* Save Data's ValueObjects */
[UnitOf(typeof(int), UnitGenerateOptions.ImplicitOperator | UnitGenerateOptions.Comparable | UnitGenerateOptions.JsonConverter)]
public readonly partial struct SaveDataId
{
}

/* User's ValueObjects */

[UnitOf(typeof(int), UnitGenerateOptions.ImplicitOperator | UnitGenerateOptions.JsonConverter)]
public readonly partial struct UserId
{
}

/* Character's ValueObjects */
[UnitOf(typeof(int), UnitGenerateOptions.ImplicitOperator | UnitGenerateOptions.JsonConverter)]
public readonly partial struct Health
{
}

[UnitOf(typeof(float), UnitGenerateOptions.ImplicitOperator | UnitGenerateOptions.JsonConverter)]
public readonly partial struct Stamina
{
}