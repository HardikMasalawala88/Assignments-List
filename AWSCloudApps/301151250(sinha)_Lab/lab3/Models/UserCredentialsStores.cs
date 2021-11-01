using lab3.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Models
{
    public static class UserCredentialsStores
    {
        static List<UserCredential> userCredentialList = null;
        static UserCredentialsStores()
        {
            if(userCredentialList == null)
                userCredentialList = new List<UserCredential>();
        }

        public static List<UserCredential> getUserList() {
            for (int i = 0; i < 3; i++)
            {
                var index = i + 1;
                userCredentialList.Add( new UserCredential {
                    Id = index,
                    Name = "user"+ index,
                    Username = "user"+ index + "@yopmail.com",
                    Password = "123123",
                });
            }
            return userCredentialList;
        }

        public static List<UserCredential> addUser(UserCredential userCredential)
        {
            userCredential.Id = userCredentialList.Count() + 1;
            userCredentialList.Add(userCredential);
            return userCredentialList;
        }
    }
}
