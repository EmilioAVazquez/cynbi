using System;
using GraphQL.Types;
using cynbi.Models;

namespace cynbi.GraphQLTypes{
    public class UserType:  ObjectGraphType<User>{
        
        public UserType()
        {
            Name = "User";

            Field(h => h.Id).Description("The id of the human.");
            Field(h => h.FirstName, nullable: true).Description("The first name of the human.");
            Field(h => h.LastName, nullable: true).Description("The last name of the human.");
        }
    }

    
    public class UserInputType:  InputObjectGraphType<User>{
        public UserInputType()
        {
            Name = "UserInput";
            Field<StringGraphType>("firstName");
            Field<StringGraphType>("lastName");
        }
    }
}