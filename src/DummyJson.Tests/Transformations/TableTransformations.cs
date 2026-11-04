using DummyJson.Tests.Tables;
using Reqnroll;

namespace DummyJson.Tests.Transformations;

[Binding]
public sealed class TableTransformations
{
    [StepArgumentTransformation]
    public static UserTable ToUserTable(DataTable table) => table.CreateInstance<UserTable>();

    [StepArgumentTransformation]
    public static NewUserTable ToNewUserTable(DataTable table) => table.CreateInstance<NewUserTable>();

    [StepArgumentTransformation]
    public static ProductTable ToProductTable(DataTable table) => table.CreateInstance<ProductTable>();

    [StepArgumentTransformation]
    public static IEnumerable<CartProductTable> ToCartProductTables(DataTable table) => table.CreateSet<CartProductTable>();
}
