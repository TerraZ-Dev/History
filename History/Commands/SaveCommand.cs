using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;
using Terraria;
using TShockAPI;
using TShockAPI.DB;

namespace History.Commands
{
	public class SaveCommand : HCommand
	{
		private Action[] actions;

		public SaveCommand(Action[] actions)
			: base(null)
		{
			this.actions = actions;
		}

		public override void Execute()
		{
			if (TShock.DB.GetSqlType() == SqlType.Sqlite)
			{
				using (IDbConnection db = History.Database.CloneEx())
				{
					db.Open();
					using (SqliteTransaction transaction = (SqliteTransaction)db.BeginTransaction())
					{
						using (SqliteCommand command = (SqliteCommand)db.CreateCommand())
						{
							foreach(Action a in actions)
                            {
								var xy = (a.x << 16) + a.y; 
								var direction = (a.direction ? 1 : -1);
								command.CommandText = $"INSERT INTO History (Time, Account, Action, XY, Data, Style, Paint, WorldID, Text, Alternate, Random, Direction) VALUES ({a.time}, {a.account}, {a.action}, {xy}, {a.data}, {a.style}, {a.paint}, {Main.worldID}, {a.text}, {a.alt}, {a.random}, {direction})";
								command.ExecuteNonQuery();

							}
						}
						transaction.Commit();
					}
				}
			}
			else
			{
				using (IDbConnection db = History.Database.CloneEx())
				{
					db.Open();
					using (MySqlTransaction transaction = (MySqlTransaction)db.BeginTransaction())
					{
						using (MySqlCommand command = (MySqlCommand)db.CreateCommand())
						{
							foreach (Action a in actions)
							{
								var xy = (a.x << 16) + a.y;
								var direction = (a.direction ? 1 : -1);
								command.CommandText = $"INSERT INTO History (Time, Account, Action, XY, Data, Style, Paint, WorldID, Text, Alternate, Random, Direction) VALUES ({a.time}, {a.account}, {a.action}, {xy}, {a.data}, {a.style}, {a.paint}, {Main.worldID}, {a.text}, {a.alt}, {a.random}, {direction})";
								command.ExecuteNonQuery();

							}
						}
						transaction.Commit();
					}
				}
			}
		}
	}
}
