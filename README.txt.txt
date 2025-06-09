I abandoned the database since it is not required. I have also switched from a website format to a console app, as that is the expected format.

-I created a bug constructor with Severity and status enums, as well as a mini comment class used in the bug class. This removes the need for a second connection BugPost table, which would've been needed in the somewhat normalized database I had.
-I made the User class, with an enum Role. Role defaults to GeneralUser.

-I want to make methods for each of the menu layers, I feel that this will be easier than the maze of if statements I usually use.
-I need to store some sample data. I think i am going to use a List<class> but i don't know exactly how I am going to do that. In theory I could have a file that contains multiple lists for different classes, but I can't find a way to that. alternatively I could make the lists in the same area as the class, this seems to be the standard.