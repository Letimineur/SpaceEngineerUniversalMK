using System;
using VRage.ModAPI;
using VRage.Utils;

namespace UniversalMK.GUI
{

	public abstract class ModdedHudStatBase : IMyHudStat
	{
		private float m_currentValue;

		private string m_valueStringCache;

		protected string m_formatter = "{0:0.00}";
		
		public MyStringHash Id { get; protected set; }

		public float CurrentValue
		{
			get
			{
				return this.m_currentValue;
			}
			protected set
			{
				if (!this.m_currentValue.IsEqualFloat(value, 0.0001f))
				{
					this.m_currentValue = value;
					this.m_valueStringCache = null;
				}
			}
		}

		public virtual float MaxValue
		{
			get
			{
				return 1f;
			}
		}

		public virtual float MinValue
		{
			get
			{
				return 0f;
			}
		}

		public abstract void Update();

		public override string ToString()
		{
			return string.Format(this.m_formatter, this.CurrentValue);
		}

		public string GetValueString()
		{
			if (this.m_valueStringCache == null)
			{
				this.m_valueStringCache = this.ToString();
			}
			return this.m_valueStringCache;
		}
	}
}
