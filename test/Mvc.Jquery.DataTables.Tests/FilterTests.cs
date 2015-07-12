using System;
using System.Collections.Generic;
using System.Linq;
using Mvc.JQuery.Datatables.CSharpModel;
using Moq;
using Xunit;
namespace Mvc.JQuery.Datatables.Tests
{
    public class FilterTests
    {

        [Theory]
        [InlineData("asdf", "", false)] //contains, not a match
        [InlineData("Archibald", "", true)] //contains, is match
        [InlineData("^Kapitein", "", true)] //contains, is match
        [InlineData("Haddock$", "", true)] //contains, is match
        [InlineData("^Archibald", "", false)] //contains, is match
        [InlineData("Archibald$", "", false)] //contains, is match
        [InlineData("^Archibald$", "", false)] //contains, is match
        [InlineData("True", "", true)] //exact query, is match
        [InlineData("False", "", false)] //exact query, is not match
        [InlineData("null", "", false)] //exact query, is not match
        [InlineData("^True$", "", true)] //exact query, is match
        [InlineData("^False$", "", false)] //exact query, isnt match
        [InlineData("^123$", "", true)] //exact query, is match
        [InlineData("^456$", "", false)] //exact query, isnt match
        [InlineData("123", "", true)] //query, is match
        [InlineData("456", "", false)] //query, isnt match
        [InlineData("2015/01/01 10:01:00", "", true)] //query, isnt match
        [InlineData("2015/01/01 10:01:01", "", false)] //query, isnt match
        [InlineData("2015/01/01 10:01:00", "When", true)] //query, isnt match
        [InlineData("123", "Cost", true)] //query, is match
        [InlineData("456", "Cost", false)] //query, isnt match

        public void SearchQueryTests(string searchString, string columnName, bool returnsResult)
        {
            var _TestData = new TestData(columnName, searchString);
            var result = new DataTables().GetRepsonse(_TestData.queryable, _TestData.dataTablesRequest);

            var data = result;
            Assert.Equal(returnsResult, data.recordsFiltered > 0);
        }
    }
    public class TestData
    {
        public DataTablesRequest dataTablesRequest;
        public IQueryable<SomeModel> queryable;
        public Tuple<string, ModelProperty>[] columns;
        public TestData(string columnName, string searchString)
        {

            queryable = new List<SomeModel>()
            {
                new SomeModel()
                {
                    Category = 1,
                    DisplayName = "Kapitein Archibald Haddock",
                    Id = 123,
                    Scale = 123.456d,
                    Discounted = true,
                    Cost = 123,
                    When = new DateTime(2015,1,1,10,01,0)
                },
                new SomeModel()
                {
                    Category = 2,
                    DisplayName = "Trifonius Zonnebloem",
                    Id = 234,
                    Scale = 234.567d,
                    Discounted = true,
                    Cost = 234,
                    When = new DateTime(2016,1,1,10,01,0)
                }
            }.AsQueryable();

            dataTablesRequest = new DataTablesRequest()
            {
                Length = 1,
                Draw = 1,
                Order = new List<DataTablesRequest.OrderT>() {
                    new DataTablesRequest.OrderT() { Column = 2, Dir = "Asc" }
                },
                Search = new DataTablesRequest.SearchT()
            };

            columns = ModelProperties<SomeModel>.Properties.Select(p =>
                    Tuple.Create(p.Name, new ModelProperty(p.PropertyInfo, new Attribute[] { }))).ToArray();
            dataTablesRequest.columns = columns.Select(c => new DataTablesRequest.ColumnT()
            {
                searchable = true,
                Orderable = true,
                data = c.Item1,
                Search = new DataTablesRequest.SearchT() { Regex = false, Value = "" }
            }).ToArray();

            var search = new DataTablesRequest.SearchT()
            {
                Value = searchString,
                Regex = false
            };
            if (columnName.Any())
                dataTablesRequest.columns.Where(c => c.data == columnName).First().Search = search;
            else
                dataTablesRequest.Search = search;
        }
    }
}

