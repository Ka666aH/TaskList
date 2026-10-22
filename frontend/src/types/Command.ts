export interface Command {
    name: string,
    func: () => void | Promise<void>
}