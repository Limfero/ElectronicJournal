import Classes from "./components/classes/Classes";
import Lessons from "./components/lessons/Lessons";
import Subjects from "./components/subjects/Subjects";
import Teachers from "./components/teachers/Teachers";
import Login from "./components/auth/Login";
import Logout from "./components/auth/Logout";
import Home from "./components/home/Home";
import Scores from "./components/scores/Scores";
import Student from "./components/student/Student";

const AppRoutes = [
  {
    path: "/home",
    element: <Home />
  },
  {
    path: '/lessons',
    element: <Lessons />
  },
  {
    path: '/subjects',
    element: <Subjects />
  },
  {
    path: '/teachers',
    element: <Teachers />
  },
  {
    path: '/classes',
    element: <Classes />
  },
  {
    path: `/student/:id`,
    element: <Student/>
  },
  {
    path: "/login",
    element: <Login/>
  },
  {
    path: '/logout',
    element: <Logout/>
  },
  {
    path: '/scores',
    element: <Scores/>
  }
];

export default AppRoutes;
