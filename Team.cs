using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsApp{
    
    public class Team{

        public string Name{get; set;}
        public int Wins{get; set;}
        public int Draws{get; set;}
        public int Losses{get; set;}
        public int Points{get; set;}
        public int GoalsFor{get; set;}
        public int GoalsAgainst{get; set;}
        public int GoalsDifference{
            get { return GoalsFor - GoalsAgainst; }
        }
        public int GamesPlayed{get; set;}

        public Team(string tname){
            Name=tname;
            Wins=0;
            Draws=0;
            Losses=0;
            Points=0;
            GoalsFor=0;
            GoalsAgainst=0;
            GamesPlayed=0;
        }       

        public Team(){
        
        }

        public void ApplyMatch(int goalsScored, int goalsConceded){

            GoalsFor += goalsScored;
            GoalsAgainst += goalsConceded;

            if (goalsScored > goalsConceded){
                Wins++;
                Points += 3;
                GamesPlayed++;
            }
            else if (goalsScored < goalsConceded){
                Losses++;
                GamesPlayed++;
            }
            else{
                Draws++;
                Points += 1;
                GamesPlayed++;
            }
        }

        public void UndoMatch(int goalsScored, int goalsConceded){
            GoalsFor -= goalsScored;
            GoalsAgainst -= goalsConceded;

            if (goalsScored > goalsConceded){
                Wins--;
                Points -= 3;
                GamesPlayed--;
            }
            else if (goalsScored < goalsConceded){
                Losses--;
                GamesPlayed--;
            }
            else{
                Draws--;
                Points -= 1;
                GamesPlayed--;
            }
        }

        public void UpdateRecord(int wins, int draws, int losses) {
            Wins = wins;
            Draws = draws;
            Losses = losses;
            Points = Wins * 3 + Draws;

        }

        public void Show(){
            Console.WriteLine($"Team: {Name} | Wins: {Wins} | Draws: {Draws} | Losses: {Losses} | "+
                              $"GF: {GoalsFor} | GA: {GoalsAgainst} | GD: {GoalsDifference} | "+
                              $"Games Played: {GamesPlayed} | Points: {Points}");
        }
    }
}