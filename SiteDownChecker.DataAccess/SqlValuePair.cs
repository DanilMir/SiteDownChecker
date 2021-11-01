namespace SiteDownChecker.DataAccess
{
    public readonly struct SqlValuePair
    {
        public string Name { get; init; }
        public object Value { get; init; }

        public SqlValuePair(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public string ValueString => Value.ToSqlString();
        public override string ToString() => $"{Name} = {ValueString}";
    }
}
