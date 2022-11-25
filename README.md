# CV

Ok, so first and foremost, this isn't really a game.
It has no score, only a few "Rooms", you can't meet other players, and you can't pick up objects or carry them around.
It is pretty basic!

**Tech**
In terms of technology, this is built with Blazor Web Assembly. I am very much not a Front-End guy, so it's pretty simplistic and likely won't work well on mobile!
The text screen is essentially just a *PRE* element that is bound to a variable that is written to a character at a time with a delay in-between each. There are a couple of mapped JS functions that are invoked to scroll to the end of the text as it is being written and to show/hide the command prompt.

The project is laid out in a Clean Architecture fashion:

 - CV - contains the main entry point to the application, the razor templates, and handles DI registration etc (largely done with *Scrutor*).   
 - CV.Application -  contains the application logic and services, including commands.   
 - CV.Domain - contains the domain model. 
 - CV.Infrastructure - handles dependencies on external resources (which in this case, is limited to downloading text files containing the game text).

Rooms are registered with the *RoomsService* on startup. Rooms have some basic state within them, and are therefore registered with a Scoped lifecycle. Rooms are intended to be self-contained. They own their own objects and doors to other rooms, and can have riddles. Whilst each conforms to the IRoom interface, they are generally free to work as they want, and maintain whatever state they want.

There is a *StateService* that handles game state - which is essentially the current room you are in, and whether it is expecting a *command* or an *answer*

The *CommandParserService* parses incoming commands to determine if they are valid, and uses the *CommandHandlerFactory* to retrieve the correct handler, which is then executed.

Objects and Doors both implement an ILookableObject interface, which makes them define a list of words that can be used to look at the object, or open the door, in order to make it more flexible for the player.

Commands return an object with multiple properties for:

 - SlowText - text that is written out slowly.
 - PreWriteStaticText - text that is written before the SlowText but all in one go - i.e. not slowly.
 - PostWriteStaticText - text that is written after the SlowText but all in one go - i.e. also not slowly.

The game is hosted in Azure and available at http://www.neildrinkall.com
