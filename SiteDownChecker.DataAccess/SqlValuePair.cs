namespace SiteDownChecker.DataAccess
{
    public readonly struct SqlValuePair
    {
        public string Name { get; init; }
        public object Value { get; init; }
        public string ValueString => Value.ToSqlString();
        public override string ToString() => $"{Name} = {ValueString}";
    }
}
