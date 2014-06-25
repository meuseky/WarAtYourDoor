using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    enum GameState { Transition, Menu, InPlay };

    enum TransitionState { Intro, Loading, LoadMenu, Outro };

    enum InPlayState { NewGame, Playing, PlayMenu, PopUpScreen, NewLevel, Death, Win };

    enum MenuState { Main, Settings, HighScore }

    enum MovementType { None, Up, Right, Down, Left };

    enum PatrolType { Guard, PatrolLine, PatrolLoop };

    enum PatrolState { Stopped, Moving, GoToFire };
}
