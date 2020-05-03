public abstract class TowerStatsList
{
	public abstract string Name { get; }
	public abstract string Discription { get; }
	public abstract float AttackDelay { get; }
	public abstract ProjectileStats Projectile { get; }
	public abstract int MaxHP { get; }
	public abstract float Range { get; }
	public abstract float Cost { get; }
	public abstract float FreezeCoef { get; }
	public abstract int PrefabId { get; }

	const float SredDMG = 250f;
	const float SredMaxHP = 1500f;
	const float SredSpeed = 4f;
	const float SredAttackCountPer20Sec = 10f;
	const float SredAttackFar = 20f;
	const float SredAttackSpeed = 2f;

	//Дамаг
	const float T2CoefDMG = 4.5f;
	const float T3CoefDMG = 6.75f;
	const float WallCoefDMG = 0f;

	//Дальность атаки
	const float T2CoefAttackFar = 1.7f;
	const float T3CoefAttackFar = 2.4f;
	const float WallCoefAttackFar = 0f;

	//АОЕ радиус
	const float T2CoefAttackAOE = 2f;
	const float T3CoefAttackAOE = 3f;
	const float WallCoefAttackAOE = 0f;

	//Макс ХП
	const float T2MaxHP = 2f;
	const float T3MaxHP = 3f;
	const float WallMaxHP = 10f;

	//Скорость снаряда (секунды)
	const float T2FlightSpeed = 1.5f;
	const float T3FlightSpeed = 2f;
	const float WallFlightSpeed = 0f;

	//Скорость атаки
	const float T2AttackSpeed = 0.85f;
	const float T3AttackSpeed = 0.7f;
	const float WallAttackSpeed = 0f;

	//
	const float TomatoDMGCoef = 0.8f;
	const float CabbageDMGCoef = 0.6f;
	const float PeasDMGCoef = 0.05f;

	const float TomatoHPCoef = 1f;
	const float CabbageHPCoef = 0.6f;
	const float PeasHPCoef = 0.15f;

	const float TomatoFarCoef = 1.2f;
	const float CabbageFarCoef = 0.7f;
	const float PeasFarCoef = 0.9f;

	const float TomatoSpeedCoef = 1.2f;
	const float CabbageSpeedCoef = 0.8f;
	const float PeasSpeedCoef = 0.7f;

	const float TomatoAOECoef = 0.4f;
	const float CabbageAOECoef = 5f;
	const float PeasAOECoef = 2f;

	const float TomatoAttackSpeedCoef = 1f;
	const float CabbageAttackSpeedCoef = 2f;
	const float PeasAttackSpeedCoef = 1.2f;

	const float CostT1 = 1000f;
	const float CostT2 = 3000f;
	const float CostT3 = 5000f;
	
	public static TowerStatsList GetStatsByPrefabId(int PrefabId)
	{
		switch (PrefabId)
		{
			case 0: return new TowerTomatoT1();
			case 1: return new TowerTomatoT2();
			case 2: return new TowerTomatoT3();
			case 3: return new TowerCabbageT1();
			case 4: return new TowerCabbageT2();
			case 5: return new TowerCabbageT3();
			case 6: return new TowerPeasT1();
			case 7: return new TowerPeasT2();
			case 8: return new TowerPeasT3();
			case 9: return new Wall();
			case 10: return new Colocol();

			default:
				return new Wall();
		}
	}

	public class TowerTomatoT1 : TowerStatsList
	{
		public override string Name => "Томатная башня";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * TomatoDMGCoef / (SredAttackSpeed * TomatoAttackSpeedCoef)) , TomatoAOECoef, SredSpeed * TomatoSpeedCoef);
		public override int MaxHP => (int)( SredMaxHP * TomatoHPCoef );		
		public override float Range => SredAttackFar* TomatoFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0;
		public override int PrefabId => 0;
	}
	public class TowerTomatoT2: TowerStatsList
	{
		public override string Name => "Томатная башня Т2";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * TomatoDMGCoef * T2CoefDMG / (SredAttackSpeed * TomatoAttackSpeedCoef * T2AttackSpeed)), TomatoAOECoef * T2CoefAttackAOE, SredSpeed * TomatoSpeedCoef * T2FlightSpeed);
		public override int MaxHP => (int)(SredMaxHP * TomatoHPCoef * T2MaxHP);
		public override float Range => SredAttackFar * TomatoFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0;
		public override int PrefabId => 1;
	}
	public class TowerTomatoT3 : TowerStatsList
	{
		public override string Name => "Томатная башня Т3";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * TomatoDMGCoef * T3CoefDMG / (SredAttackSpeed * TomatoAttackSpeedCoef * T3AttackSpeed)), TomatoAOECoef * T3CoefAttackAOE, SredSpeed * TomatoSpeedCoef * T3FlightSpeed);
		public override int MaxHP => (int)(SredMaxHP * TomatoHPCoef * T3MaxHP);
		public override float Range => SredAttackFar * TomatoFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0;
		public override int PrefabId => 2;
	}
	public class TowerCabbageT1 : TowerStatsList
	{
		public override string Name => "Капустная башня";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * CabbageDMGCoef / (SredAttackSpeed * CabbageAttackSpeedCoef)), CabbageAOECoef, SredSpeed * CabbageSpeedCoef);
		public override int MaxHP => (int)(SredMaxHP * CabbageHPCoef);
		public override float Range => SredAttackFar * CabbageFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0;
		public override int PrefabId => 3;
	}
	public class TowerCabbageT2 : TowerStatsList
	{
		public override string Name => "Томатная башня Т2";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * CabbageDMGCoef * T2CoefDMG / (SredAttackSpeed * CabbageAttackSpeedCoef * T2AttackSpeed)), CabbageAOECoef * T2CoefAttackAOE, SredSpeed * CabbageSpeedCoef * T2FlightSpeed);
		public override int MaxHP => (int)(SredMaxHP * CabbageHPCoef * T2MaxHP);
		public override float Range => SredAttackFar * CabbageFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0;
		public override int PrefabId => 4;
	}

	public class TowerCabbageT3 : TowerStatsList
	{
		public override string Name => "Томатная башня Т3";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * CabbageDMGCoef * T3CoefDMG / (SredAttackSpeed * CabbageAttackSpeedCoef * T3AttackSpeed)), CabbageAOECoef * T3CoefAttackAOE, SredSpeed * CabbageSpeedCoef * T3FlightSpeed);
		public override int MaxHP => (int)(SredMaxHP * CabbageHPCoef * T3MaxHP);
		public override float Range => SredAttackFar * CabbageFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0;
		public override int PrefabId => 5;
	}

	public class TowerPeasT1 : TowerStatsList
	{
		public override string Name => "Гороховая башня";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * PeasDMGCoef / (SredAttackSpeed * PeasAttackSpeedCoef)), PeasAOECoef, SredSpeed * PeasSpeedCoef, 0.8f, 2f);
		public override int MaxHP => (int)(SredMaxHP * PeasHPCoef);
		public override float Range => SredAttackFar * PeasFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0.7f;
		public override int PrefabId => 6;
	}
	public class TowerPeasT2 : TowerStatsList
	{
		public override string Name => "Гороховая башня Т2";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * PeasDMGCoef * T2CoefDMG / (SredAttackSpeed * PeasAttackSpeedCoef * T2AttackSpeed)), PeasAOECoef * T2CoefAttackAOE, SredSpeed * PeasSpeedCoef * T2FlightSpeed, 0.7f, 2.5f);
		public override int MaxHP => (int)(SredMaxHP * PeasHPCoef * T2MaxHP);
		public override float Range => SredAttackFar * PeasFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0.6f;
		public override int PrefabId => 7;
	}

	public class TowerPeasT3 : TowerStatsList
	{
		public override string Name => "Гороховая башня Т3";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)(SredDMG * PeasDMGCoef * T3CoefDMG / (SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed)), PeasAOECoef * T3CoefAttackAOE, SredSpeed * PeasSpeedCoef * T3FlightSpeed, 0.7f, 3f);
		public override int MaxHP => (int)(SredMaxHP * PeasHPCoef * T3MaxHP);
		public override float Range => SredAttackFar * PeasFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0.5f;
		public override int PrefabId => 8;
	}
	public class Wall : TowerStatsList
	{
		public override string Name => "Стена";
		public override string Discription => "Очень колючая и сухая.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats(0, 0f, 0f, 0f, 0f);
		public override int MaxHP => (int)(10000);
		public override float Range => 0;
		public override float Cost => 20000;
		public override float FreezeCoef => 0;
		public override int PrefabId => 9;
	}

	public class Colocol : TowerStatsList
	{
		public override string Name => "Колокол";
		public override string Discription => "Блестящий и звонкий.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats(0, 0f, 0f, 0f, 0f);
		public override int MaxHP => (int)(1000000);
		public override float Range => 0;
		public override float Cost => 0;
		public override float FreezeCoef => 0;
		public override int PrefabId => 10;
	}

}
