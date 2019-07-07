using System;
using GraphQL.Types;
using cynbi.GraphQLTypes;
using cynbi.Data;
using cynbi.Models;
using GraphQL;

namespace cynbi.GraphQL{
    public class PlatformSchema : Schema{
        public PlatformSchema(IDependencyResolver resolver) : base(resolver){
            Query = resolver.Resolve<PlatformQuery>();
            Mutation = resolver.Resolve<PlatformMutation>();
        }
    }

    public class PlatformQuery: ObjectGraphType<object>{
        public PlatformQuery(IUserRepository userRepo)
        {
            /*This section contains the graphQL query endpoints*/
            Name = "Query";

            Field<ListGraphType<UserType>>(
                "users", 
                resolve: context => userRepo.GetAll()
            );        

            Field<UserType>(
                "user", 
                resolve: context => userRepo.GetAll()[0]
            );  
        }
    }

    public class PlatformMutation: ObjectGraphType<object>{
        public PlatformMutation(IUserRepository userRepo)
        {
            /*This section contains the graphQL query endpoints*/
            Name = "Mutation";

            Field<UserType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<UserInputType> {Name = "user"}
                ),
                resolve: context =>
                {
                    var human = context.GetArgument<User>("user");
                    human.Id = userRepo.Size();
                    return userRepo.Add(human);
                }
            );      
        }
    }
}