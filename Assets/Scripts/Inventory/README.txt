Be sure to run ItemDatabase at the very beginning of the game.

When creating a new item (weapon, consumable, etc.), right-click on the project window and click Create -> 'Weapon','Consumable', etc.
A new asset representing a new item should have been made. Rename to anything you want.
The asset will have an array with fields that dictate the modifier for each stat. Change to your desired modifiers.
Once done, add the object you created into the ItemDatabase object located in Scripts/Inventory by clicking and dragging into it.

You can also retrieve a specific item in the database by using something like a LINQ query to save yourself some trouble:


// For example, you can retrieve a weapon using this method

public static Weapon GetWeaponByName(string _name)
{
	Weapon value = ItemDatabase.Weapons.First(item => item.Title == title);
	return value;
}