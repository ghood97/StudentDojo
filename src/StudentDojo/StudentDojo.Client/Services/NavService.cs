using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;

namespace StudentDojo.Client.Services;

public interface INavService
{
    string Uri { get; }

    string RelativeUri
    {
        get => new Uri(Uri).PathAndQuery;
    }

    void RedirectToResources();

    void NavigateTo([StringSyntax("Uri")] string uri, [StringSyntax("Uri")] string? redirectUri = null, bool forceLoad = false);

    void NavigateTo([StringSyntax("Uri")] string uri, NavigationOptions options, [StringSyntax("Uri")] string? redirectUri = null);

    void UpdateQueryParam(string name, string? value);

    void UpdateQueryParams(IReadOnlyDictionary<string, object?> parameters);

    Uri ToAbsoluteUri([StringSyntax("Uri")] string uri);
}

public class NavService : INavService
{
    private readonly NavigationManager _nav;

    public string Uri => _nav.Uri;

    public NavService(NavigationManager nav)
    {
        _nav = nav;
    }

    public void NavigateTo(
        [StringSyntax("Uri")] string uri,
        NavigationOptions options,
        [StringSyntax("Uri")] string? redirectUri = null)
    {
        if (redirectUri is not null)
        {
            uri = QueryHelpers.AddQueryString(uri, "redirectUri", redirectUri);
        }

        _nav.NavigateTo(uri, options);
    }

    public void NavigateTo(
        [StringSyntax("Uri")] string uri,
        [StringSyntax("Uri")] string? redirectUri = null,
        bool forceLoad = false)
    {
        NavigationOptions options = default;
        if (forceLoad)
        {
            options = new NavigationOptions
            {
                ForceLoad = true
            };
        }

        if (redirectUri is not null)
        {
            uri = QueryHelpers.AddQueryString(uri, "redirectUri", redirectUri);
        }

        _nav.NavigateTo(uri, options);
    }

    public void UpdateQueryParam(string name, string? value)
    {
        _nav.NavigateTo(_nav.GetUriWithQueryParameter(name, value), new NavigationOptions()
        {
            ReplaceHistoryEntry = true
        });
    }

    public void UpdateQueryParams(IReadOnlyDictionary<string, object?> parameters)
    {
        _nav.NavigateTo(_nav.GetUriWithQueryParameters(parameters), new NavigationOptions()
        {
            ReplaceHistoryEntry = true
        });
    }

    public void RedirectToResources()
    {
        string returnUri = new Uri(_nav.Uri).PathAndQuery;
        _nav.NavigateTo($"/Manage?redirectUri={System.Uri.EscapeDataString(returnUri)}");
    }

    public Uri ToAbsoluteUri([StringSyntax("Uri")] string uri) => _nav.ToAbsoluteUri(uri);
}
