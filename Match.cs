using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsApp{
     public class Match{
        
        public Team HomeTeam {get;}
        public Team AwayTeam {get;}
        public int HomeScore {get;}  
        public int AwayScore {get;}

        public Match(Team homeTeam, Team awayTeam,int homeScore,int awayScore){

            HomeTeam=homeTeam;
            AwayTeam=awayTeam;
            HomeScore=homeScore;
            AwayScore=awayScore;
        }  

        public void ApplyResult(){

            HomeTeam.ApplyMatch(HomeScore, AwayScore);
            AwayTeam.ApplyMatch(AwayScore, HomeScore);
        }

        public void UndoResult(){

            HomeTeam.UndoMatch(HomeScore, AwayScore);
            AwayTeam.UndoMatch(AwayScore, HomeScore);
        }
    }
}
