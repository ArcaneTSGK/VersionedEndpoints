using Microsoft.OpenApi.Models;

namespace VersionedEndpoints.Example.Endpoints;

internal static class ApiEndpoints
{
    internal static class Games
    {
        private const string RootCollection = "games";

        internal static OpenApiTag Tag => new() { Name = "Game" };

        internal const string ListGames = RootCollection;

        internal static class V1
        {
            internal const int Version = 1;
            internal const string GetGame = $"{RootCollection}/{{id:int}}";
        }

        internal static class V2
        {
            internal const int Version = 2;
            internal const string GetGame = $"{RootCollection}/{{id:int}}";
        }
    }
}
