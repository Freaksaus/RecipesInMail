﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace RecipesInMail;

/// <summary>The mod entry point.</summary>
internal sealed class ModEntry : Mod
{
	private MailFrameworkMod.Api.IMailFrameworkModApi? _mailApi;

	private IEnumerable<RecipeData> _recipeData = new List<RecipeData>()
	{
		new(1, "Stir Fry", "Stir Fry! It's a perfect way to get some healthy greens on your plate. Sautee the greens in a little sesame oil and make sure to add plenty of fresh ginger and garlic. Now breathe deeply... Oh, that's good."),
		new(2, "Coleslaw", "Coleslaw! Envisioning bland mounds of limp cabbage? You're not alone. But a great coleslaw can be so much more. Make sure you have juicy fresh cabbage for this one. Toss with a little vinegar and mayonnaise and you're all set. Ah, that's crisp."),
		new(3, "Radish Salad", "Radish Salad! There's nothing like a fresh, peppery radish. It reminds me of the late spring... my mother would slice up our fresh radishes and serve them on grainy bread with a little salt and pepper. Ah, those were the days... but I digress. Now, watch closely..."),
		new(4, "Omelet", "Omelet! This is such a simple dish, but so often done incorrectly! A perfect omelet is a thing of beauty. It's a pure golden angel of gleaming egg, and I'm going to show you my secret method..."),
		new(5, "Baked Fish", "Baked Fish! Whenever I make this one at home, my two cats go bananas. I need a second chef around just to keep them from hopping on the counter for a bite! Just make sure the fish is fresh, preferably caught with your own rod! Now watch as I place the fish on a bed of spring greens..."),
		new(5, "Pancakes", "Pancakes! Sometimes I get carried away with fancy, complicated dishes... but there's something comforting about a simple pancake. You'll want plenty of butter and maple syrup to slather over these warm stacks... mmmm...."),
		new(6, "Maki Roll", "Maki Roll! The delicate flavor of the ocean, sealed within a pillowy cloud of rice. Maybe a bit of wasabi for the adventurous. My mouth is watering as we speak. Oh, how I love sushi..."),
		new(7, "Bread", "Bread! Breadmaking can be a very complex form of art, but I'll make it simple for you. The most important ingredient is flour, of course. But before we create the dough we'll need to activate our yeast. They prefer warm water and a little something to feed on. I use plain sugar, but honey or maple syrup will do!"),
		new(8, "Tortilla", "Tortillas! How many of you are gnawing on a microwaved convenience-burrito while watching this program? Shame on you! You might as well wrap your beans in an old wash rag. Now, listen up. Once you've tried my fresh, rustic corn tortillas you'll never go back..."),
		new(9, "Trout Soup", "Trout Soup! There's something about fresh-caught trout that just gets me buzzing. Maybe it's the subtle taste of the river. At any rate, I've got a wonderful trout soup recipe to share with you today..."),
		new(10, "Glazed Yams", "Glazed Yams! It's yam season, and what better way to enjoy these delightful tubers than by slathering them in a sweet, sticky glaze? You'll need a nice, ripe yam and a whole bunch of sugar. Now, here's how you do it..."),
		new(11, "Artichoke Dip", "Artichoke Dip! This is a delightful way to get more artichokes into your body. Personally, I don't even dip anything in there. I just guzzle the sauce down like it's a milkshake. Delicious."),
		new(12, "Plum Pudding", "Plum Pudding! Little Jack Horner is chomping at the bit for this ooey-gooey delight. And who can blame him? The plums are floral and sweet, with just the right amount of tartness to fully activate every last one of your eager taste buds. You'll just have to try it and see for yourself..."),
		new(13, "Chocolate Cake", "Chocolate Cake! What better way to spend these cold winter nights than digging your way through a rich chocolate mountain? With my help, you'll be well on your way to creating the finest cake you've ever laid eyes on. Now, you're going to need a lot of sugar..."),
		new(14, "Pumpkin Pie", "Pumpkin Pie! In my house, it's a tradition to eat pumpkin pie during the Feast of the Winter Star. The season just isn't complete without that wonderful flavor you can only achieve with the finest farm-fresh pumpkins. A little nutmeg, cinnamon, and clove will sweep you off to a land of crimson and gold..."),
		new(15, "Cranberry Candy", "Cranberry Candy! Here's a fun one to help you celebrate the new year. Cranberries are quite bitter on their own, but submerge them in an ocean of sugar they'll taste like angel's tears. Here's how you do it..."),
		new(16, "Pizza", "Pizza! There's a reason pizza is a timeless culinary classic. You've got an artisan golden-brown crust, you've got a tangy, garlic-infused marinara sauce, and you're topping it all with a mouth-watering three-cheese blend. And we're just getting started! Let's throw some fresh pepper and tomato on the top. Oh, my!"),
		new(17, "Hashbrowns", "Hashbrowns! This one's simple, but that's a good thing! You'll just want to shred some potatoes, add a liberal amount of salt, and fry to perfection in your favorite high-heat oil. Sounds easy, right? Well, I've got some pointers that'll make your job a lot easier..."),
		new(18, "Complete Breakfast", "Complete Breakfast! Last week I taught you how to make hashbrowns. This week I'll show you how to combine them with other ingredients to create a hearty complete breakfast. This meal will really fill you up and give you the energy you need to get in a hard day's work."),
		new(19, "Lucky Lunch", "Lucky Lunch! An old legend has it that this meal is irresistible to spirits of luck and fortune. After consuming it, you're likely to attract a good spirit into your belly, where it will bless you with good fortune until the meal is digested! Sounds weird, huh? I can't say I believe it, but the meal is delicious nonetheless!"),
		new(20, "Carp Surprise", "Hey, ever have a bunch of carp laying around and no idea what to do with them? Yeah, me too. Well, I've devised a great solution to this all-too-common problem. I call it... Carp surprise. It's quite easy to make, but you'll need a lot of carp..."),
		new(21, "Maple Bar", "Maple Bars! Ever notice how the maple bars are always first to disappear from the doughnut box? Perplexing, because your average maple bar tastes like a sweet hockey puck. Agree? Well, just wait until you try my recipe. You can really taste the forest in these bars. Okay, you're going to need real, quality maple syrup..."),
		new(22, "Pink Cake", "A viewer from Pelican Town wrote to me recently... let's see... Her name's Haley. She wrote, 'I tried your pink cake last time I was in Zuzu City and I fell in love with it. Could you share the recipe on your next episode?'. Well, why not? It's a marvelous cake. And you'll never guess the secret ingredient..."),
		new(23, "Roasted Hazelnuts", "Roasted Hazelnuts! I've got a nice old hazelnut tree behind my house, and every year I invite the family over for a nut roasting party! Once we start roasting, it's inevitable that the neighbors will show up. That rich, nutty smell is irresistible. Now, here's the family recipe..."),
		new(24, "Fruit Salad", "Fruit Salad! Here's a healthy and delicious treat to brighten up your day. The most important thing to remember is that you need ripe fruit. Forget the bland stuff they sell at the supermarket... I'm talking fresh-picked, juicy, bursting-with-flavor fruit. Okay, now watch closely..."),
		new(25, "Blackberry Cobbler", "Blackberry Cobbler! This one always reminds me of Stardew Valley. There's a few days in fall where the valley is overflowing with the most delicious blackberries I've ever tasted. If you can get your hands on some, I'd highly recommend them for this simple cobbler."),
		new(26, "Crab Cakes", "Crab Cakes! Crab meat is very flimsy on its own, but mixing it with bread crumbs and egg is a great way to give them some body. That's why these cakes are my favorite way to eat crab! But before you go cracking any shells, stay tuned for my essential seasoning mixture..."),
		new(27, "Fiddlehead Risotto", "Fiddlehead Risotto! Fiddlehead ferns are beautiful, but actually quite bland on their own. That's why you have to pay careful attention to the spices you add. I'll be honest, it's not very easy to make a good fiddlehead risotto. But with my help, you'll have the best chance at achieving it."),
		new(28, "Poppyseed Muffin", "Poppyseed Muffin! Poppies make beautiful ornamental flowers... but why not make full use of the plant with these delightful muffins? I'm a huge fan of poppy seeds. They're subtle, but they add a nice flavor and a great texture to bready desserts. And they're healthy!"),
		new(29, "Lobster Bisque", "Lobster Bisque! You could serve this one to the governer himself. It's rich, creamy and delicious, with just the right amount of oceanic flavor. The hardest part is finding some lobster, but I'm sure you can do it. Heck, if you're feeling crafty you could catch one yourself with a crab pot!"),
		new(30, "Bruschetta", "Bruschetta! It's a wonderfully simple appetizer that really showcases the quality of the ingredients. You'll need bread, tomato, and oil. First, you'll need to grill the bread. Slice the tomato and place onto the grilled bread. Then drizzle with oil and serve. I told you it was simple!"),
		new(31, "Shrimp Cocktail", "Shrimp Cocktail! Here's another good appetizer for you and your dinner guests. My cocktail sauce is extra zesty, and I'm sure you'll love it. Now, the first step..."),
	};

	public override void Entry(IModHelper helper)
	{
		helper.Events.GameLoop.GameLaunched += OnGameLaunched;
	}

	private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
	{
		_mailApi = Helper.ModRegistry.GetApi<MailFrameworkMod.Api.IMailFrameworkModApi>("DIGUS.MailFrameworkMod");
		if (_mailApi is null)
		{
			return;
		}

		foreach (var recipe in _recipeData)
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
}
