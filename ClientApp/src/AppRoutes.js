import Classes from "./components/Classes";
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Lessons from "./components/Lessons";
import Student from "./components/Student";
import Subjects from "./components/Subjects";
import Teachers from "./components/Teachers";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
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
    path: `/student/${id}`,
    element: <Student idStudent={id} />
  }
];

export default AppRoutes;
