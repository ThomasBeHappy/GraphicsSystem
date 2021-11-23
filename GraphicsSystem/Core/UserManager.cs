using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class UserManager
    {
        public static bool firstSetup = false;

        public static User loggedInUser;
        public static bool loggedIn = false;

        public static void Initialize()
        {
            if (!Directory.Exists(@"0:\Users"))
            {
                Directory.CreateDirectory(@"0:\Users");
                firstSetup = true;
            }
        }

        public static User Login(string username, string password)
        {
            if (Directory.Exists(@"0:\Users\" + username))
            {
                byte[] usercnf = File.ReadAllBytes(@"0:\Users\" + username + @"\user.cnf");

                int perm = usercnf[0];

                char[] hashedPassword = new char[usercnf.Length-1];

                for (int i = 1; i < usercnf.Length; i++)
                {
                    hashedPassword[i-1] = (char)usercnf[i];
                }

                if (Hashing.Hashing.verifyHash(password, hashedPassword.ToString()))
                {
                    User user = new User(username.ToCharArray(), perm);
                    loggedIn = true;
                    loggedInUser = user;
                    return user;
                }
            }

            return null;
        }


        public static bool CreateUser(string username, string password, int permLevel = 1)
        {
            if (Directory.Exists(@"0:\Users\" + username))
            {
                return false;
            }

            Directory.CreateDirectory(@"0:\Users\" + username);

            byte[] data = new byte[password.Length + 1];
            data[0] = (byte)permLevel;

            int i = 1;
            foreach (var item in password)
            {
                data[i] = (byte)item;
                i++;
            }

            File.WriteAllBytes(@"0:\Users\" + username + @"\user.cnf", data);

            return true;
        }
    }

    public class User
    {
        public char[] username;
        public int permLevel = 1; // 0 = SYSTEM, 1 = User, 2 = Guest

        public User(char[] username, int permLevel)
        {
            this.username = username;
            this.permLevel = permLevel;
        }
    }
}
