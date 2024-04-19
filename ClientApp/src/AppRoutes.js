import Classes from "./components/classes/Classes";
import Lessons from "./components/lessons/Lessons";
import Subjects from "./components/subjects/Subjects";
import Teachers from "./components/teachers/Teachers";
import Student from "./components/student/Student";
import Login from "./components/auth/Login";
import Logout from "./components/auth/Logout";
import Home from "./components/home/Home";

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
  }
];

export default AppRoutes;
