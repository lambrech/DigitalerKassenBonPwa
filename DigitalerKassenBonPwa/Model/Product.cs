using System;

namespace DigitalerKassenBonPwa.Model;

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

public class Product
{
    [JsonInclude]
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; } = 1.5m;

    public string Description { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is Product other)
        {
            return Equals(this.Id, other.Id);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}

public class ShoppingCart
{
    [JsonInclude]
    public Dictionary<Guid, int> ProductsInCart { get; private set; } = new();
}

public class DataContainer
{
    [JsonInclude]
    public HashSet<Product> Products { get; private set; } = new ();

    [JsonInclude]
    public ShoppingCart ShoppingCart { get; private set; } = new ();
}



public interface IDataProvider
{
    Task Save();

    Task<DataContainer> GetCurrent();
}

public class DataProvider : IDataProvider
{
    private const string DataBaseKey = "DiKaBo_Local_Database";

    private readonly ILocalStorageService localStorageService;

    public DataContainer? DataContainer { get; private set; }

    public DataProvider(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public async Task Save()
    {
        //var json = JsonSerializer.Serialize(this.DataContainer);
        //await this.localStorageService.SetItemAsync(DataBaseKey, json);
        await this.localStorageService.SetItemAsync(DataBaseKey, this.DataContainer);
        this.DataContainerSaved?.Invoke(this, EventArgs.Empty);
    }

    public async Task<DataContainer> GetCurrent()
    {
        if (this.DataContainer != null)
        {
            return this.DataContainer;
        }

        if (!await this.localStorageService.ContainKeyAsync(DataBaseKey))
        {
            this.DataContainer = new DataContainer();
            await this.Save();
        }

        //var json = await this.localStorageService.GetItemAsync<string>(DataBaseKey);
        //this.DataContainer = JsonSerializer.Deserialize<DataContainer>(json);
        
        this.DataContainer = await this.localStorageService.GetItemAsync<DataContainer>(DataBaseKey);
        Debug.Assert(this.DataContainer != null);

        this.DataContainerChanged?.Invoke(this, this.DataContainer);
        return this.DataContainer;
    }

    public event EventHandler<DataContainer>? DataContainerChanged;
    
    public event EventHandler? DataContainerSaved;
}

//// ReSharper disable All
//[JsonSerializable(typeof(Product))]
//internal partial class MyJsonProductContext : JsonSerializerContext
//{
//}

//[JsonSerializable(typeof(ShoppingCart))]
//internal partial class MyJsonShoppingCartContext : JsonSerializerContext
//{
//}

//[JsonSerializable(typeof(DataContainer))]
//internal partial class MyJsonDataContainerContext : JsonSerializerContext
//{
//}
//// ReSharper enable All1