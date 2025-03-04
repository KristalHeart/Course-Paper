using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace Курсач
{
    public class SaveLoad
    {
        public double EnemyHp;
        public double PlayerHp;
        public void Save(Enemy enemy, Player player)
        {
            EnemyHp = enemy.HP;
            PlayerHp = player.HP;
        }
        public void Load(Enemy enemy, Player player) 
        {
            enemy.HP = EnemyHp;
            player.HP = PlayerHp;
        }
    }
    public class Enemy
    {
        public virtual string Name { get; }
        public double HP { get; set; }
        public double AttackStrength { get; set; }
        public bool EnemyDefense = false;
        public Enemy(string name, double hp, double attackStrength)
        {
            Name = name;
            HP = hp;
            AttackStrength = attackStrength;
        }
        public void Attack(Player player)
        {
            if (player.PlayerDefense)
            {
                player.HP = player.HP - AttackStrength * 0.5;
            }
            else
            {
                player.HP = player.HP - AttackStrength;
            }
            EnemyDefense = false;
        }
        public void Defend()
        {
            EnemyDefense = true;
        }
    }
    public class Player
    {
        public double HP = 20;
        public double AttackStrength = 5;
        public bool PlayerDefense= false;
        public void Attack(Enemy enemy)
        {
            if (enemy.EnemyDefense == true)
            {
                enemy.HP = enemy.HP - AttackStrength * 0.5;
            }
            else
            {
                enemy.HP = enemy.HP - AttackStrength;
            }
            PlayerDefense = false;
        }
        public void Defend()
        {
            PlayerDefense= true;
        }
    }
    public class Monster : Enemy
    {
        public override string Name => "Монстр";
        public Monster(double hp, double attackStrength) : base("Монстр", 10, 8) { }
    }
    public class Rogue1 : Enemy
    {
        
        public override string Name => "Вор";
        public Rogue1(double hp, double attackStrength) : base("Вор", 8, 10) { }
        public bool DefenseCount = false;
        public void Action(Player player)
        { if (DefenseCount) 
            {
                Defend();
                DefenseCount = false;
            }
        else
            {
               Attack(player);
               DefenseCount= true;
           }
        }
    }
    public class Guardian1 : Enemy
    {
        public override string Name => "Стражник";
        public Guardian1(double hp, double attackStrength) : base("Стражник", 10, 50) { }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void MonsterGame(object sender, RoutedEventArgs e)
        {
            Window2 GameWindow = new Window2();
            GameWindow.Show();
        }
        private void RogueGame(object sender, RoutedEventArgs e)
        {
            Rogue GameWindow = new Rogue();
            GameWindow.Show();
        }
        private void GuardianGame(object sender, RoutedEventArgs e)
        {
            Guardian GameWindow = new Guardian();
            GameWindow.Show();
        }
    }
    public partial class Rogue : Window
    {
        Rogue1 rogue = new Rogue1(8, 10);
        Player player = new Player();
        SaveLoad saveload = new SaveLoad();
        private async void Attack(object sender, RoutedEventArgs e)
        {
            rogue.Action(player);
            player.Attack(rogue);
            if (rogue.DefenseCount==false)
            {
                Actions.Text += "\nВы атакуете вора. Вор уходит в защиту. Вы наносите ему 2,5 урона";
            }
            else
            {
                Actions.Text += "\nВор атакует вас. Вы теряете 10 HP";
            }
            RogueStats.Text = $"Rogue Health: {rogue.HP}, Rogue Attack: {rogue.AttackStrength}";
            
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (rogue.HP <= 0)
            {
                rogue.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private async void Defend(object sender, RoutedEventArgs e)
        {
            player.Defend();
            Actions.Text += "\nВы уходите в защиту";
            rogue.Action(player);
            if (rogue.DefenseCount==false)
            {
                Actions.Text += "\nВор уходит в защиту";
            }
            else
            {
                Actions.Text += "\nВор атакует вас. Вы в защите. Вы теряете 5 HP";
            }
            RogueStats.Text = $"Rogue Health: {rogue.HP}, Rogue Attack: {rogue.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (rogue.HP <= 0)
            {
                rogue.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            saveload.Save(rogue, player);
            Actions.Text += $"\nСохранение выполнено.";
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            saveload.Load(rogue, player);
            Actions.Text =  "";
            Actions.Text += "\nЗагрузка выполнена.";
            RogueStats.Text = $"Rogue Health: {rogue.HP}, Rogue Attack: {rogue.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
        }
    }
    public partial class Guardian : Window
    {
        Guardian1 guard = new Guardian1(10, 50);
        Player player = new Player();
        SaveLoad saveload = new SaveLoad();
        private async void Attack(object sender, RoutedEventArgs e)
        {
            guard.Defend();
            Actions.Text += "\nСтраж уходит в защиту";
            player.Attack(guard);
            Actions.Text += "\nВы атакуете стража. Он теряет 2,5 HP";
            GuardianStats.Text = $"Guardian Health: {guard.HP}, Guardian Attack: {guard.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (guard.HP <= 0)
            {
                guard.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private async void Defend(object sender, RoutedEventArgs e)
        {
            guard.Defend();
            Actions.Text += "\nСтраж уходит в защиту";
            player.Defend();
            Actions.Text += "\nВы уходите в защиту";
            GuardianStats.Text = $"Guardian Health: {guard.HP}, Guardian Attack: {guard.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (guard.HP <= 0)
            {
                guard.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            saveload.Save(guard, player);
            Actions.Text += "\nСохранение выполнено.";
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            saveload.Load(guard, player);
            Actions.Text = "";
            Actions.Text += "\nЗагрузка выполнена.";
            GuardianStats.Text = $"Guardian Health: {guard.HP}, Guardian Attack: {guard.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
        }
    }
    public partial class Window2 : Window
    {
        Monster monster = new Monster(11, 8);
        Player player = new Player();
        SaveLoad saveload = new SaveLoad();
        private async void Attack(object sender, RoutedEventArgs e)
        {
            player.Attack(monster);
            Actions.Text += "\nВы атакуете монстра. Он теряет 5 HP";
            MonsterStats.Text = $"Monster Health: {monster.HP}, Monster Attack: {monster.AttackStrength}";
            monster.Attack(player);
            Actions.Text += "\nМонстр атакует вас. Вы теряете 8 HP";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (monster.HP <= 0)
            {
                monster.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private async void Defend(object sender, RoutedEventArgs e)
        {
            player.Defend();
            Actions.Text += "\nВы уходите в защиту";
            monster.Attack(player);
            Actions.Text += "\nМонстр атакует вас. Вы в защите. Вы теряете 4 HP";
            MonsterStats.Text = $"Monster Health: {monster.HP}, Monster Attack: {monster.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
            if (player.HP <= 0)
            {
                player.HP = 0;
                Result.Text = "Вы проиграли";
                await Task.Delay(1000);
                this.Close();
            }
            if (monster.HP <= 0)
            {
                monster.HP = 0;
                Result.Text = "Вы выиграли";
                await Task.Delay(1000);
                this.Close();
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            saveload.Save(monster, player);
            Actions.Text += "\nСохранение выполнено.";
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            saveload.Load(monster, player);
            Actions.Text = "";
            Actions.Text += "\nЗагрузка выполнена.";
            MonsterStats.Text = $"Monster Health: {monster.HP}, Monster Attack: {monster.AttackStrength}";
            PlayerStats.Text = $"player Health: {player.HP}, Player Attack {player.AttackStrength}";
        }
    }
}