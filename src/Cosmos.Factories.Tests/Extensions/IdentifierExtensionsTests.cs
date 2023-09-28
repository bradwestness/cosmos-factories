namespace Cosmos.Factories.Tests.Extensions;
using System;
using Cosmos.Factories.Extensions;

public sealed class IdentifierExtensionsTests
{
    [Fact]
    public void GetIdentifier_Null_ReturnsExpected()
    {
        var input = (object)null!;

        var actual = input.GetIdentifier();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.True(Guid.TryParse(actual, out var actualGuid));
        Assert.NotEqual(Guid.Empty, actualGuid);
    }

    [Fact]
    public void GetIdentifier_Property_NullOrEmpty_ReturnsExpected()
    {
        var input = new MyPropertyType
        {
            Id = string.Empty
        };

        var actual = input.GetIdentifier();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.True(Guid.TryParse(actual, out var actualGuid));
        Assert.NotEqual(Guid.Empty, actualGuid);
    }

    [Fact]
    public void GetIdentifier_IdProperty_ReturnsExpected()
    {
        var input = new MyPropertyType
        {
            Id = "fizz",
            NamedProperty = "buzz",
        };

        var actual = input.GetIdentifier();

        Assert.NotNull(actual);
        Assert.Equal("fizz", actual);
    }

    [Fact]
    public void GetIdentifier_NamedProperty_ReturnsExpected()
    {
        var input = new MyPropertyType
        {
            Id = "fizz",
            NamedProperty = "buzz",
        };

        var actual = input.GetIdentifier(nameof(input.NamedProperty));

        Assert.NotNull(actual);
        Assert.Equal("buzz", actual);
    }

    [Fact]
    public void GetIdentifier_Field_NullOrEmpty_ReturnsExpected()
    {
        var input = new MyFieldType
        {
            Id = string.Empty
        };

        var actual = input.GetIdentifier();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.True(Guid.TryParse(actual, out var actualGuid));
        Assert.NotEqual(Guid.Empty, actualGuid);
    }

    [Fact]
    public void GetIdentifier_IdField_ReturnsExpected()
    {
        var input = new MyFieldType
        {
            Id = "fizz",
            NamedField = "buzz",
        };

        var actual = input.GetIdentifier();

        Assert.NotNull(actual);
        Assert.Equal("fizz", actual);
    }

    [Fact]
    public void GetIdentifier_NamedField_ReturnsExpected()
    {
        var input = new MyFieldType
        {
            Id = "fizz",
            NamedField = "buzz",
        };

        var actual = input.GetIdentifier(nameof(input.NamedField));

        Assert.NotNull(actual);
        Assert.Equal("buzz", actual);
    }

    private class MyPropertyType
    {
        public string Id { get; set; }

        public string NamedProperty { get; set; }
    }

    private class MyFieldType
    {
        public string Id;

        public string NamedField;
    }
}
