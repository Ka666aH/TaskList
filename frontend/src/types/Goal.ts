export interface Goal {
  id: string
  title: string
  description: string | null
  createAt: string
  deadline: string | null
}
export type GoalRequest = Omit<Goal, 'id' | 'createAt'>
