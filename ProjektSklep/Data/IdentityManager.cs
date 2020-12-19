using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjektSklep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Data
{/*
    public class IdentityManager
	{
		public RoleManager<IdentityRole> LocalRoleManager
		{
			get
			{
				return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ShopContext()));
			}
		}


		public UserManager<Customer> LocalUserManager
		{
			get
			{
				return new UserManager<Customer>(new UserStore<Customer>(new ShopContext()));
			}
		}


		public Customer GetUserByID(string userID)
		{
			Customer user = null;
			UserManager<Customer> um = this.LocalUserManager;

			user = um.FindById(userID);

			return user;
		}


		public ApplicationUser GetUserByName(string email)
		{
			ApplicationUser user = null;
			UserManager<ApplicationUser> um = this.LocalUserManager;

			user = um.FindByEmail(email);

			return user;
		}


		public bool RoleExists(string name)
		{
			var rm = LocalRoleManager;

			return rm.RoleExists(name);
		}


		public bool CreateRole(string name)
		{
			var rm = LocalRoleManager;
			var idResult = rm.CreateAsync(new IdentityRole(name));

			return idResult.IsCompletedSuccessfully;
		}


		public bool CreateUser(Customer user, string password)
		{
			var um = LocalUserManager;
			var idResult = um.CreateAsync(user, password);

			return idResult.IsCompletedSuccessfully;
		}


		public bool AddUserToRole(string userId, string roleName)
		{
			var um = LocalUserManager;
			var idResult = um.AddToRole(userId, roleName);

			return idResult.Succeeded;
		}


		public bool AddUserToRoleByUsername(string username, string roleName)
		{
			var um = LocalUserManager;

			string userID = um.FindByName(username).Id;
			var idResult = um.AddToRole(userID, roleName);

			return idResult.Succeeded;
		}


		public void ClearUserRoles(string userId)
		{
			var um = LocalUserManager;
			var user = um.FindById(userId);
			var currentRoles = new List<IdentityUserRole>();

			currentRoles.AddRange(user.Roles);

			foreach (var role in currentRoles)
			{
				um.RemoveFromRole(userId, role.RoleId);
			}
		}
	}*/
}
