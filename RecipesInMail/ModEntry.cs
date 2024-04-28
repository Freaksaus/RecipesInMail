using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace RecipesInMail;

/// <summary>The mod entry point.</summary>
internal sealed class ModEntry : Mod
{
	public override void Entry(IModHelper helper)
	{
		helper.Events.GameLoop.GameLaunched += OnGameLaunched;
	}

	private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
	{
		var mailApi = Helper.ModRegistry.GetApi<MailFrameworkMod.Api.IMailFrameworkModApi>("DIGUS.MailFrameworkMod");
		if (mailApi is null)
		{
			return;
		}

		var recipes = LoadRecipesFromData();
		foreach (var recipe in recipes)
		{
			var days = recipe.WeekNumber * 7;
			var letter = new MailFrameworkMod.Letter(
				$"{recipe.UniqueName}-via-mail",
				recipe.Description,
				recipe.UniqueName,
			(x) => Game1.Date.TotalDays == days &&
				  !Game1.player.mailReceived.Contains(x.Id) &&
				  !Game1.player.cookingRecipes.Keys.Contains(recipe.UniqueName),
			(x) => Game1.player.mailReceived.Add(x.Id));

			MailFrameworkMod.MailRepository.SaveLetter(letter);
		}
	}

	private List<RecipeData> LoadRecipesFromData()
	{
		var recipes = new List<RecipeData>();
		var recipeData = Game1.content.Load<Dictionary<string, string>>("Data\\TV\\CookingChannel");
		foreach (KeyValuePair<string, string> entry in recipeData)
		{
			var values = entry.Value.Split('/');
			if (values.Length < 2)
			{
				Monitor.Log("Unable to parse the cooking channel recipes", LogLevel.Error);
				return recipes;
			}

			if (!int.TryParse(entry.Key, out var weekNumber))
			{
				Monitor.Log("Unable to parse the cooking channel recipes", LogLevel.Error);
				return recipes;
			}
			recipes.Add(new(weekNumber, values[0], values[1]));
		}

		return recipes;
	}
}
