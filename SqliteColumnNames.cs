//utilities for generating column name strings based on a https://github.com/praeclarum/sqlite-net model
public static class ColumnName<T>
{
    /// <summary>
    /// PropertyName
    /// </summary>
    public static string Of<TValue>(
        Expression<Func<T, TValue>> selector)
    {
        Expression body = selector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression) body).Body;
        }
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var name = ((PropertyInfo) ((MemberExpression) body).Member).Name;
                    //TODO: support attribute controlled names
                return name;
            default:
                throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// TableName.PropertyName
    /// </summary>
    public static string QualifiedOf<TValue>(
        Expression<Func<T, TValue>> selector)
    {
        Expression body = selector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression) body).Body;
        }
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var type = ((MemberExpression) body).Expression.Type;
                var attrib =
                    type.GetCustomAttributes(typeof (TableAttribute), true).FirstOrDefault() as TableAttribute;
                var name = attrib == null ? type.Name : attrib.Name;
                name += ".";
                name += ((PropertyInfo) ((MemberExpression) body).Member).Name;
                    //TODO: support attribute controlled names
                return name;
            default:
                throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// outer.PropertyName
    /// </summary>
    public static string QualifiedOf<TValue>(
        string outer,
        Expression<Func<T, TValue>> selector)
    {
        Expression body = selector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression) body).Body;
        }
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var name = outer;
                name += ".";
                name += ((PropertyInfo) ((MemberExpression) body).Member).Name;
                    //TODO: support attribute controlled names
                return name;
            default:
                throw new InvalidOperationException();
        }
    }
}

public static class ColumnNameAs<T, U> {
    /// <summary>
    /// TableName.PropertyName AS asProp
    /// </summary>
    public static string For<TValue>(
        Expression<Func<T, TValue>> selector,
        Expression<Func<U, TValue>> destSelector
        )
    {
        Expression body = selector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression)body).Body;
        }
        string name;
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var type = ((MemberExpression)body).Expression.Type;
                var attrib = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
                name = attrib == null ? type.Name : attrib.Name;
                name += ".";
                name += ((PropertyInfo)((MemberExpression)body).Member).Name; //TODO: support attribute controlled names
                break;
            default:
                throw new InvalidOperationException();
        }
        body = destSelector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression)body).Body;
        }
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                name += " AS " +  ((PropertyInfo)((MemberExpression)body).Member).Name; //TODO: support attribute controlled names
                break;
            default:
                throw new InvalidOperationException();
        }
        return name;
    }
    /// <summary>
    /// outer.PropertyName AS asProp
    /// </summary>
    public static string For<TValue>(string outer, 
        Expression<Func<T, TValue>> selector,
        Expression<Func<U, TValue>> destSelector
        )
    {
        Expression body = selector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression)body).Body;
        }
        string name;
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                name = outer;
                name += ".";
                name += ((PropertyInfo)((MemberExpression)body).Member).Name; //TODO: support attribute controlled names
                break;
            default:
                throw new InvalidOperationException();
        }
        body = destSelector;
        if (body is LambdaExpression)
        {
            body = ((LambdaExpression)body).Body;
        }
        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                name += " AS " + ((PropertyInfo)((MemberExpression)body).Member).Name; //TODO: support attribute controlled names
                break;
            default:
                throw new InvalidOperationException();
        }
        return name;
    }

}
