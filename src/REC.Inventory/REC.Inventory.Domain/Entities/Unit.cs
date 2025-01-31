﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REC.Inventory.Domain.Entities
{
	public class Unit : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
	}
}
