using System;
using System.Collections.Generic;
using cynbi.Models;

namespace cynbi.Data{

  public interface IUserRepository{
    User Add(User user);
    List<User> GetAll();
    int Size();
  }
  public class UserRepository: IUserRepository{
    private List<User> users;
    private static int i;

    public UserRepository(){
      users = new List<User>();
      Seed();
    }

    public User Add(User user){
      users.Add(user);
      return user;
    }

    public List<User> GetAll(){
      return users;
    }

    public int Size(){
      return users.Count;
    }

    private void Seed(){
      users.Add(new User{Id = 0, FirstName = "emilio", LastName = "vazquez"});
      users.Add(new User{Id = 1, FirstName = "miguel", LastName = "cervantes"});
      users.Add(new User{Id = 2, FirstName = "mario", LastName = "benedetti"});
    }
  }
}
